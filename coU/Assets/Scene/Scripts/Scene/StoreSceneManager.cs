using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreSceneManager : MonoBehaviour
{
    public static string storeName = "사봉";
    public static string categoryMain = "뷰티";
    public static string categorySub = "바디&향수";
    public static string floor = "";

    List<Store> store;
    List<Item> item_List;
    GameObject Menu;
    int backCount = 0;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        if (Stack.Instance.Count() == 0)
            GameObject.Find("Btn_Back").SetActive(false);
        Scene s = SceneManager.GetSceneByName("StoreScene");
        GameObject[] gameObjects = s.GetRootGameObjects();
        print(gameObjects.Length);
        foreach (GameObject it in gameObjects)
        {
            print(it.name);
            if (it.name == "Canvas_Main")
                Menu = it.gameObject.transform.Find("Panel_Whole/Panel_Main/ScrollView_Main/Viewport/Content/Panel_MenuParent/Panel_Menu").gameObject;
            if (Login.Instance.IsPermission(storeName))
                if (it.name == "Panel_Img")
                    it.transform.Find("Btn_Upload").gameObject.SetActive(true);
        }
        Debug.Log("StoreSceneManager start: StoreName " + storeName);
        Debug.Log("StoreSceneManager start: categorySub " + categorySub);
        InitialContent();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (SceneManager.sceneCount == 2 && SceneManager.GetActiveScene().name == "MaxstScene"))
        {
            if (Stack.Instance.Count() > 0)
            {
                BackBtnOnClick();
            }
            else
            {
                backCount++;
                if (!IsInvoking("ResetBackCount"))
                    Invoke("ResetBackCount", 1.0f);
                if (backCount == 2)
                {
                    CancelInvoke("ResetBackCount");
                    Application.Quit();
#if !UNITY_EDITOR
	System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
                }
                Toast.ShowToastMessage("한 번 더 누르시면 종료됩니다.", 250);
            }

        }
    }

    void ResetBackCount()
    {
        backCount = 0;
    }

    void InitialContent()
    {
        string query = "Select * from Stores Where name = '" + storeName + "'";
        query += " AND categorySub like '%" + categorySub + "%'"; //빈 문자열일 수도 있으니까
        Debug.Log("StoreSceneManager query = " + query);

        store = GetDBData.getStoresData(query);

        GameObject.Find("TMP_Name").GetComponent<TextMeshProUGUI>().text = store[0].name;
        GameObject.Find("TMP_CtMain").GetComponent<TextMeshProUGUI>().text = store[0].categoryMain + "/" + store[0].categorySub;
        GameObject.Find("TMP_Floor").GetComponent<TextMeshProUGUI>().text = store[0].floor;
        GameObject.Find("TMP_Phone").GetComponent<TextMeshProUGUI>().text = store[0].phoneNumber;
        GameObject.Find("TMP_Hour").GetComponent<TextMeshProUGUI>().text = store[0].openHour;
        storeName = store[0].name;
        categoryMain = store[0].categoryMain;
        categorySub = store[0].categorySub;
        floor = store[0].floor;

        //이미지 띄우는 부분
        Image img = GameObject.Find("Img_Logo").gameObject.GetComponent<Image>();
        string path = store[0].logoPath;
        path = path.Substring(0, path.Length - 4);
        Texture2D texture = Resources.Load(path, typeof(Texture2D)) as Texture2D;
        if (texture == null)
            texture = Resources.Load("default_logo", typeof(Texture2D)) as Texture2D;
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);
        Debug.Log("name is " + store[0].name);
        InitialMenu(store[0].name);
    }

    void InitialMenu(string sname)
    {
        string query = "select items.* from items " +
                " inner join stores " +
                " where stores.tntSeq = items.tntSeq " +
                " and stores.name = '" + store[0].name + "' " +
                " group by (itemTitle) " +
                " order by `index`";
        Debug.Log("StoreScene Menu query " + query);
        item_List = GetDBData.getItemsData(query);

        Debug.Log("StoreScene Number of Menu " + item_List.ToArray().Length.ToString());
        if (item_List.ToArray().Length < 1)
            Menu.SetActive(false);
        else
        {
            Menu.SetActive(true);
            GameObject MenuList= Menu.transform.Find("Panel_MenuList").gameObject;
            int maxCount = 3;
            int price;
            if (item_List.ToArray().Length < maxCount)
                maxCount = item_List.ToArray().Length;
            GameObject MenuNamePanel = MenuList.transform.Find("Panel_MenuName").gameObject;
            GameObject MenuPricePanel = MenuList.transform.Find("Panel_MenuPrice").gameObject;
            for (int i = 1; i <= maxCount; i++)
            {
                string menu = item_List[i - 1].itemTitle;
                if (item_List[i - 1].itemTitleSub != null)
                    menu += "(" + item_List[i - 1].itemTitleSub + ")";
                MenuNamePanel.transform.Find("TMP_MenuName" + i.ToString()).GetComponent<TextMeshProUGUI>().text = menu;
                price = item_List[i - 1].itemPrice;
                MenuPricePanel.transform.Find("TMP_MenuPrice" + i.ToString()).GetComponent<TextMeshProUGUI>().text = string.Format("{0:n0}", price) + "원";
            }
        }
    }

    void BackBtnOnClick()
    {
        SceneInfo before = Stack.Instance.Pop();
        string beforePath = SceneUtility.GetScenePathByBuildIndex(before.beforeScene);
        if (beforePath.Contains("MaxstScene"))
        {
            Stack.Instance.Clear();
            SceneManager.UnloadSceneAsync("StoreScene");
        }
        else
        {
            if (beforePath.Contains("SearchScene"))
                SearchSceneManager.searchStr = before.storeName;
            else if (beforePath.Contains("StoreListScene"))
                StoreListSceneManager.categorySub = before.categorySub;
            else //MaxstScene으로 가던, AllCategoryScene으로 가던 스택 비워줘야 함.
                Stack.Instance.Clear();
            SceneManager.LoadScene(before.beforeScene);
        }
    }
}
