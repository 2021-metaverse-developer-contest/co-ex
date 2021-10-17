using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SearchSceneManager : MonoBehaviour
{
    public GameObject itemList;
    public GameObject itemResult;
    GameObject inputObject;
    //GameObject inputText;
    TMP_InputField inputOuter;
    //TextMeshProUGUI inputInner;
    Button btnSearch;

    GameObject[] items;
    GameObject[] results;
    public static string searchStr = "";
    public static int beforeScene;

    // Start is called before the first frame update
    void Start()
    {
        inputObject = GameObject.Find("InputTMP_Search").gameObject;
        inputOuter = inputObject.GetComponent<TMP_InputField>();
        //inputText = GameObject.Find("TMP_Input");
        //inputInner = inputText.GetComponent<TextMeshProUGUI>();
        btnSearch = GameObject.Find("Btn_Search").GetComponent<Button>();

        inputOuter.onSubmit.AddListener(delegate { SearchBtnOnClick(inputOuter.text); });
        inputOuter.onValueChanged.AddListener(delegate { ShowList(inputOuter.text); });
        inputOuter.onEndEdit.AddListener(delegate { ShowList(inputOuter.text); });
        inputOuter.onSelect.AddListener(delegate { FocusInputField(); });
        btnSearch.onClick.AddListener(delegate { SearchBtnOnClick(inputOuter.text); });

        Debug.Log("SearchSceneManager start: beforeSceneIndex " + beforeScene.ToString());
        if (searchStr != "")
        {
            SearchBtnOnClick(searchStr);
            inputOuter.text = searchStr;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            backBtnClick();
    }

    void backBtnClick()
    {
        //SceneManager.LoadScene("CategoryScene");
        SceneManager.LoadScene(beforeScene);
    }

    public void ShowList(string inputText)
    {
        print("inputOuter " + inputOuter.text);
        searchStr = inputText;
        for (int i = 0; items != null && i < items.Length; i++)
            DestroyImmediate(items[i]);
        string query = "Select * from Stores where name like '%" + inputText.Trim() + "%'";
        query += "group by name order by name ASC";
        List<Stores> stores = DBConnect.CustomExcuteQuery(query);
        items = new GameObject[stores.ToArray().Length];
        print("items number = " + items.Length);
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = Instantiate(itemList, GameObject.Find("Content_List").transform);
            items[i].GetComponentInChildren<TextMeshProUGUI>().text = stores[i].name;
        }
    }

    public void ShowResult(string inputText)
    {
        for (int i = 0; results != null && i < results.Length; i++)
            DestroyImmediate(results[i]);
        string query = "Select * from Stores where name like '%" + inputText.Trim() + "%'";
        query += "group by name order by name ASC";
        List<Stores> stores = DBConnect.CustomExcuteQuery(query);
        results = new GameObject[stores.ToArray().Length];
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = Instantiate(itemResult,GameObject.Find("Content_Result").transform);
            results[i].transform.Find("TMP_Result").GetComponent<TextMeshProUGUI>().text = stores[i].name;
            results[i].transform.Find("TMP_Floor").GetComponent<TextMeshProUGUI>().text = stores[i].floor;

            Image img = results[i].transform.Find("Img_Result").GetComponent<Image>();
            if (img == null)
                Debug.Log("image is null");
            string imgPath = stores[i].logoPath;
            imgPath = imgPath.Substring(0, imgPath.Length - 4);
            print("imgPath = " + imgPath);
            Texture2D texture = Resources.Load(imgPath, typeof(Texture2D)) as Texture2D;
            if (texture == null)
                texture = Resources.Load("default_logo", typeof(Texture2D)) as Texture2D;
            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);
        }
    }

    public void FocusInputField()
    {
        print("Focus is changed");
        inputOuter.ActivateInputField();
        inputOuter.Select();
        GameObject.Find("Panel_List").transform.Find("ScrollView_List").gameObject.SetActive(true);
        GameObject.Find("Panel_List").transform.Find("ScrollView_Result").gameObject.SetActive(false);
        ShowList(inputOuter.text);
    }

    public void SearchBtnOnClick(string inputText)
    {
        GameObject.Find("Panel_List").transform.Find("ScrollView_List").gameObject.SetActive(false);
        GameObject.Find("Panel_List").transform.Find("ScrollView_Result").gameObject.SetActive(true);
        ShowResult(inputText);
    }
}
