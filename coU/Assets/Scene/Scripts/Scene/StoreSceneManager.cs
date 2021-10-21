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
    public static bool beforeScene = false; //어느 경로로 왔는지에 따라 달라짐 true-StoreListScene, false-SearchScene
    public static string searchStr = "";
    public static string floor = "";

    //hyojlee 2021/10/21
    //위치공유 시 링크 눌렀을 때 StoreScene이 제일 먼저 실행되므로 이를 확인하기 위해서
    public static bool isMaxst = false;

    List<Store> store;
    List<Item> item_List;
    GameObject Menu;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        Menu = GameObject.Find("Panel_Menu").gameObject;
        Debug.Log("StoreSceneManager start: StoreName " + storeName);
        Debug.Log("StoreSceneManager start: categorySub " + categorySub);
        Debug.Log("StoreSceneManager before: " + beforeScene.ToString());
        InitialContent();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backBtnClick();
        }
    }

    void InitialContent()
    {
        string query = "Select * from Stores Where name = '" + storeName + "'";
        if (beforeScene)
            query += " AND categorySub = '" + categorySub + "'";
        Debug.Log("StoreSceneManager query = " + query);

        store = GetDBData.getStoresData(query);

        GameObject.Find("TMP_Name").GetComponent<TextMeshProUGUI>().text = store[0].name;
        GameObject.Find("TMP_CtMain").GetComponent<TextMeshProUGUI>().text = store[0].categoryMain + "/" + store[0].categorySub;
        GameObject.Find("TMP_Floor").GetComponent<TextMeshProUGUI>().text = store[0].floor;
        GameObject.Find("TMP_Phone").GetComponent<TextMeshProUGUI>().text = store[0].phoneNumber;
        GameObject.Find("TMP_Hour").GetComponent<TextMeshProUGUI>().text = store[0].openHour;
        StoreSceneManager.storeName = store[0].name;
        StoreSceneManager.categoryMain = store[0].categoryMain;
        StoreSceneManager.categorySub = store[0].categorySub;
        StoreSceneManager.floor = store[0].floor;

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
                "inner join stores " +
                "where stores.tntSeq = items.tntSeq " +
                "and stores.name = '" + store[0].name + "' " +
                "group by (itemTitle) " +
                "order by `index`";
        item_List = GetDBData.getItemsData(query);

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

    void backBtnClick()
    {
        if (isMaxst)
        {
            isMaxst = false;
            SceneManager.LoadScene("MaxstScene");
        }
        else
        {
            if (beforeScene)
                SceneManager.LoadScene("StoreListScene");
            else
                SceneManager.LoadScene("SearchScene");
        }
    }

    //public void MapBtnOnClick()
    //{
    //    // General:m.starfield.co.kr/coexmall/tenant/tenantDetail/TN201904161630148787?maps=eyJzaG93VHlwZSI6MCwiZGV0YWlsIjp7InRudF9zZXEiOiJUTjIwMTkwNDE2MTYzMDE0ODc4NyIsIm1hcF9pZCI6IlBPLXVnQmxRc1RRQTUwNDYifSwiZmxvb3IiOiJCMSJ9
    //    // Possible:m.starfield.co.kr/coexmall/?maps=
    //    string query = "Select * from Stores Where name = '" + storeName + "'";
    //    if (categorySub != "")
    //        query += " AND categorySub = '" + categorySub + "'";
    //    IDbConnection conn = new SqliteConnection(DBConnect.GetDBFilePath());
    //    conn.Open();
    //    IDbCommand cmd = conn.CreateCommand();
    //    cmd.CommandText = query;
    //    IDataReader reader = cmd.ExecuteReader();
    //    reader.Read();
    //    // 별마당도서관의 reader["mapKey"] = null 이기때문에 예외처리가 필요함.
    //    string MapdeepLink = "https://m.starfield.co.kr/coexmall/tenant/tenantDetail/" + reader["tntSeq"].ToString() + "?maps=" + reader["mapKey"].ToString();
    //    Application.OpenURL(MapdeepLink);
    //    reader.Dispose();
    //    cmd.Dispose();
    //    conn.Dispose();
    //}

    //public void PhoneBtnOnclick()
    //{
    //    GameObject clickObj = EventSystem.current.currentSelectedGameObject;
    //    string phone = clickObj.GetComponentInParent<TextMeshProUGUI>().text.Trim() ;

    //    Application.OpenURL("tel:" + phone);
    //    print("tel:" + phone);
    //}
}
