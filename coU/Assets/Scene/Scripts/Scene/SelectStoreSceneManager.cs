using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SelectStoreSceneManager : MonoBehaviour
{
    public GameObject itemList;
    public GameObject itemResult;
    GameObject inputObject;
    TMP_InputField inputOuter;
    Button btnSearch;

    GameObject[] items;
    static GameObject[] results;
    public static string searchStr = "";

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        inputObject = GameObject.Find("InputTMP_SearchSelect").gameObject;
        inputOuter = inputObject.GetComponent<TMP_InputField>();
        btnSearch = GameObject.Find("Btn_SearchSelect").GetComponent<Button>();

        inputOuter.onSubmit.AddListener(delegate { SearchBtnOnClick(inputOuter.text); });
        inputOuter.onValueChanged.AddListener(delegate { ShowList(inputOuter.text); });
        inputOuter.onEndEdit.AddListener(delegate { ShowList(inputOuter.text); });
        inputOuter.onSelect.AddListener(delegate { FocusInputField(); });
        btnSearch.onClick.AddListener(delegate { SearchBtnOnClick(inputOuter.text); });

        if (searchStr != "")
        {
            SearchBtnOnClick(searchStr);
            inputOuter.text = searchStr;
        }

    }

    public void ShowList(string inputText)
    {
        print("inputOuter " + inputOuter.text);
        searchStr = inputText;
        for (int i = 0; items != null && i < items.Length; i++)
            DestroyImmediate(items[i]);
        string query = "Select * from Stores where name like '%" + inputText.Trim() + "%'";
        query += "group by name order by name ASC";
        List<Store> stores = GetDBData.getStoresData(query);
        items = new GameObject[stores.ToArray().Length];
        print("items number = " + items.Length);
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = Instantiate(itemList, GameObject.Find("Content_ListSelect").transform);
            items[i].GetComponentInChildren<TextMeshProUGUI>().text = stores[i].name;
        }
    }

    public void ShowResult(string inputText)
    {
        for (int i = 0; results != null && i < results.Length; i++)
            DestroyImmediate(results[i]);
        string query = "Select * from Stores where name like '%" + inputText.Trim() + "%'";
        query += " group by name order by name ASC";

        List<Store> stores = GetDBData.getStoresData(query);
        results = new GameObject[stores.ToArray().Length];
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = Instantiate(itemResult,GameObject.Find("Content_ResultSelect").transform);
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
        Debug.Log("Focus is changed");
        inputOuter.ActivateInputField();
        inputOuter.Select();
        GameObject.Find("Panel_ListSelect").transform.Find("ScrollView_ListSelect").gameObject.SetActive(true);
        GameObject.Find("Panel_ListSelect").transform.Find("ScrollView_ResultSelect").gameObject.SetActive(false);
        ShowList(inputOuter.text);
    }

    public void SearchBtnOnClick(string inputText)
    {
        GameObject.Find("Panel_ListSelect").transform.Find("ScrollView_ListSelect").gameObject.SetActive(false);
        GameObject.Find("Panel_ListSelect").transform.Find("ScrollView_ResultSelect").gameObject.SetActive(true);
        ShowResult(inputText);
    }
}
