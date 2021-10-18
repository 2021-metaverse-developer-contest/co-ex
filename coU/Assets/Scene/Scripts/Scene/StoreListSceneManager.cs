using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreListSceneManager : MonoBehaviour
{
    public GameObject item;
    public static string categorySub = "";

    GameObject f1;
    GameObject b1;
    GameObject b2;
    List<Stores> f1_list;
    List<Stores> b1_list;
    List<Stores> b2_list;

    GameObject[] f1_items;
    GameObject[] b1_items;
    GameObject[] b2_items;
    GameObject PanelFloor;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        GameObject.Find("TMP_SubCategory").GetComponent<TextMeshProUGUI>().text = categorySub;
        f1 = GameObject.Find("ScrollView_F1").gameObject;
        b1 = GameObject.Find("ScrollView_B1").gameObject;
        b2 = GameObject.Find("ScrollView_B2").gameObject;
        PanelFloor = GameObject.Find("Panel_Floor").gameObject;
        Debug.Log("StoreListSceneManager start: categorySub " + categorySub);

        FillContent();
        ChkNoDataFloor();
        if (b1_items.Length > f1_items.Length)
        {
            if (b1_items.Length < b2_items.Length)
                StoreListClick.clickFloor = "B2";
            else
                StoreListClick.clickFloor = "B1";
        }
        else
        {
            if (f1_items.Length < b2_items.Length)
                StoreListClick.clickFloor = "B2";
            else
                StoreListClick.clickFloor = "F1";
        }
        StoreListClick.FloorBtnOnClick();
        //f1.SetActive(false);
        //b1.SetActive(false);
        //b2.SetActive(false);
        //if (b1_items.Length > f1_items.Length)
        //{
        //    if (b1_items.Length < b2_items.Length)
        //        b2.SetActive(true);
        //    else
        //        b1.SetActive(true);
        //}
        //else
        //{
        //    if (f1_items.Length < b2_items.Length)
        //        b2.SetActive(true);
        //    else
        //        f1.SetActive(true);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("AllCategoryScene");
    }

    void ChkNoDataFloor()
    {
        if (b1_items.Length == 0)
            PanelFloor.transform.Find("Btn_B1").gameObject.SetActive(false);
        if (f1_items.Length == 0)
            PanelFloor.transform.Find("Btn_F1").gameObject.SetActive(false);
        if (b2_items.Length == 0)
            PanelFloor.transform.Find("Btn_B2").gameObject.SetActive(false);
    }

    void FillContent()
    {
        int i;
        string defaultLogoPath = "default_logo";
        string query = "Select * from Stores where categorySub = '" + categorySub + "'";
        query += " AND floor = '";

        f1_list = getDBData.getStoresData(query + "1F'");
        b1_list = getDBData.getStoresData(query + "B1'");
        b2_list = getDBData.getStoresData(query + "B2'");

        f1_items = new GameObject[f1_list.ToArray().Length];
        b1_items = new GameObject[b1_list.ToArray().Length];
        b2_items = new GameObject[b2_list.ToArray().Length];

        for (i = 0; i < f1_items.Length; i++)
        {
            f1_items[i] = Instantiate(item, f1.transform.Find("Viewport").Find("Content"));
            Image img = f1_items[i].transform.Find("Panel_Icon").Find("Img_Icon").GetComponent<Image>();
            Debug.Log("path is " + f1_list[i].logoPath.Substring(0, f1_list[i].logoPath.Length - 4));
            Texture2D texture = Resources.Load(f1_list[i].logoPath.Substring(0, f1_list[i].logoPath.Length - 4), typeof(Texture2D)) as Texture2D;
            if (texture == null)
                texture = Resources.Load(defaultLogoPath, typeof(Texture2D)) as Texture2D;
            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);
            f1_items[i].transform.Find("Panel_Name").GetComponentInChildren<TextMeshProUGUI>().text = f1_list[i].name;
        }
        for (i = 0; i < b1_items.Length; i++)
        {
            b1_items[i] = Instantiate(item, b1.transform.Find("Viewport").Find("Content"));
            Image img = b1_items[i].transform.Find("Panel_Icon").Find("Img_Icon").GetComponent<Image>();

            Texture2D texture = Resources.Load(b1_list[i].logoPath.Substring(0, b1_list[i].logoPath.Length - 4), typeof(Texture2D)) as Texture2D;
            if (texture == null)
                texture = Resources.Load(defaultLogoPath, typeof(Texture2D)) as Texture2D;
            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);
            b1_items[i].transform.Find("Panel_Name").GetComponentInChildren<TextMeshProUGUI>().text = b1_list[i].name;
        }
        for (i = 0; i < b2_items.Length; i++)
        {
            b2_items[i] = Instantiate(item, b2.transform.Find("Viewport").Find("Content"));
            Image img = b2_items[i].transform.Find("Panel_Icon").Find("Img_Icon").GetComponent<Image>();
            Texture2D texture = Resources.Load(b2_list[i].logoPath.Substring(0, b2_list[i].logoPath.Length - 4), typeof(Texture2D)) as Texture2D;
            if (texture == null)
                texture = Resources.Load(defaultLogoPath, typeof(Texture2D)) as Texture2D;
            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);
            b2_items[i].transform.Find("Panel_Name").GetComponentInChildren<TextMeshProUGUI>().text = b2_list[i].name;
        }
    }
}
