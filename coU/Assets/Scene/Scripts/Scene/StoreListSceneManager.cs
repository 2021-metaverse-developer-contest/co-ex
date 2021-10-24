using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using maxstAR;

public class StoreListSceneManager : MonoBehaviour
{
    public GameObject item;
    public static string categorySub = "";

    GameObject f1;
    GameObject b1;
    GameObject b2;
    List<Store> f1_list;
    List<Store> b1_list;
    List<Store> b2_list;

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            BackBtnOnClick();
  //      addDistance(f1_list);
  //      addDistance(b1_list);
  //      addDistance(b2_list);
  //      int i = 0;
		//for (i = 0; i < f1_items.Length; i++)
		//	f1_items[i].transform.Find("Panel_Name/Tmp_Name/Tmp_Distance").GetComponent<TextMeshProUGUI>().text = (Math.Ceiling((f1_list[i].distance / 10f)) * 10).ToString() + "m";
		//for (i = 0; i < b1_items.Length; i++)
		//	b1_items[i].transform.Find("Panel_Name/Tmp_Name/Tmp_Distance").GetComponent<TextMeshProUGUI>().text = (Math.Ceiling((b1_list[i].distance / 10f)) * 10).ToString() + "m";
		//for (i = 0; i < b2_items.Length; i++)
		//	b2_items[i].transform.Find("Panel_Name/Tmp_Name/Tmp_Distance").GetComponent<TextMeshProUGUI>().text = (Math.Ceiling((b2_list[i].distance / 10f)) * 10).ToString() + "m";
    }

    void BackBtnOnClick()
    {
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

    private Action<List<Store>> addDistance = (storeList) =>
    {
        foreach (Store i in storeList)
        {
            Vector3 vStore = new Vector3((float)i.modifiedX, 0f, (float)i.modifiedY);
            i.distance = Vector3.Distance(vStore, MaxstSceneManager.vAR);
        }
    };

    Action<int> action = (b) =>
    {
        Console.WriteLine(b);
    };

    int cmp(Store x, Store y)
    {
        if (x.distance < y.distance)
            return -1;
        else
            return 1;
    }

    void FillContent()
    {
        int i;
        string defaultLogoPath = "default_logo";
        string query = "Select * from Stores where categorySub = '" + categorySub + "'";
        query += " AND floor = '";

        f1_list = GetDBData.getStoresData(query + "1F'");
        b1_list = GetDBData.getStoresData(query + "B1'");
        b2_list = GetDBData.getStoresData(query + "B2'");

        // 순회하면서 계산 distance에 값넣기
        addDistance(f1_list);
        addDistance(b1_list);
        addDistance(b2_list);
        // 넣은 값을 기준으로 정렬하기
        f1_list.Sort(cmp);
        b1_list.Sort(cmp);
        b2_list.Sort(cmp);

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
            f1_items[i].transform.Find("Panel_Name/Tmp_Name").GetComponent<TextMeshProUGUI>().text = f1_list[i].name;
            f1_items[i].transform.Find("Panel_Name/Tmp_Name/Tmp_Distance").GetComponent<TextMeshProUGUI>().text = "";
        }
        for (i = 0; i < b1_items.Length; i++)
        {
            b1_items[i] = Instantiate(item, b1.transform.Find("Viewport").Find("Content"));
            Image img = b1_items[i].transform.Find("Panel_Icon").Find("Img_Icon").GetComponent<Image>();

            Texture2D texture = Resources.Load(b1_list[i].logoPath.Substring(0, b1_list[i].logoPath.Length - 4), typeof(Texture2D)) as Texture2D;
            if (texture == null)
                texture = Resources.Load(defaultLogoPath, typeof(Texture2D)) as Texture2D;
            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);
            b1_items[i].transform.Find("Panel_Name/Tmp_Name").GetComponent<TextMeshProUGUI>().text = b1_list[i].name;
            b1_items[i].transform.Find("Panel_Name/Tmp_Name/Tmp_Distance").GetComponent<TextMeshProUGUI>().text = "";
        }
        for (i = 0; i < b2_items.Length; i++)
        {
            b2_items[i] = Instantiate(item, b2.transform.Find("Viewport").Find("Content"));
            Image img = b2_items[i].transform.Find("Panel_Icon").Find("Img_Icon").GetComponent<Image>();
            Texture2D texture = Resources.Load(b2_list[i].logoPath.Substring(0, b2_list[i].logoPath.Length - 4), typeof(Texture2D)) as Texture2D;
            if (texture == null)
                texture = Resources.Load(defaultLogoPath, typeof(Texture2D)) as Texture2D;
            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);
            b2_items[i].transform.Find("Panel_Name/Tmp_Name").GetComponent<TextMeshProUGUI>().text = b2_list[i].name;
            b2_items[i].transform.Find("Panel_Name/Tmp_Name/Tmp_Distance").GetComponent<TextMeshProUGUI>().text = "";
        }
         StartCoroutine(UpdateDistance());
    }
    IEnumerator UpdateDistance()
    {
        int i;
		
        while (true)
        {
            Debug.Log("Distance Update");
            if (MaxstSceneManager.onceDetectARLocation == true)
            {
                addDistance(f1_list);
                addDistance(b1_list);
                addDistance(b2_list);
                for (i = 0; i < f1_items.Length; i++)
                    f1_items[i].transform.Find("Panel_Name/Tmp_Name/Tmp_Distance").GetComponent<TextMeshProUGUI>().text = (Math.Ceiling((f1_list[i].distance / 10f)) * 10).ToString() + "m";
                for (i = 0; i < b1_items.Length; i++)
                    b1_items[i].transform.Find("Panel_Name/Tmp_Name/Tmp_Distance").GetComponent<TextMeshProUGUI>().text = (Math.Ceiling((b1_list[i].distance / 10f)) * 10).ToString() + "m";
                for (i = 0; i < b2_items.Length; i++)
                    b2_items[i].transform.Find("Panel_Name/Tmp_Name/Tmp_Distance").GetComponent<TextMeshProUGUI>().text = (Math.Ceiling((b2_list[i].distance / 10f)) * 10).ToString() + "m";
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
