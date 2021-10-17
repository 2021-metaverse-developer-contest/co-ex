using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using SQLite4Unity3d;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreListScene1Manager : MonoBehaviour
{
    public GameObject storeFactory;
    public static string categorySub = "한식";
    string categoryMain = "";
    // Start is called before the first frame update
    void Start()
    {
        string query = "Select * from Stores where categorySub = '" + categorySub + "'";
        IDbConnection conn = new SqliteConnection(DBConnect.GetDBFilePath());
        conn.Open();
        IDbCommand cmd = conn.CreateCommand();
        cmd.CommandText = query;
        IDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            categoryMain = reader["categoryMain"].ToString();
            GameObject store = Instantiate(storeFactory, GameObject.Find("Content").transform);
            store.GetComponentInChildren<TextMeshProUGUI>().text = reader["name"].ToString();
        }
        reader.Dispose();
        cmd.Dispose();
        conn.Dispose();

        //string dbPath = Application.streamingAssetsPath + "/stores_v1.db";
        //var db = new SQLiteConnection(dbPath);
        //List<SQLiteConnection.ColumnInfo> storesDB = db.GetTableInfo("Stores");

        //var items = db.Query<Stores>("Select * from Stores where categorySub = '" + categorySub+ "'");
        //foreach (var item in items)
        //{
        //    print(item.name);
        //    GameObject category = Instantiate(storeFactory, GameObject.Find("Content").transform);
        //    category.GetComponentInChildren<Text>().text = item.name + "\t\t" + item.floor;
        //}
        //db.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backBtnClick();
        }
    }

    void backBtnClick()
    {
        SceneManager.LoadScene("SubCategoryScene");
        SubCtSceneManager.categoryMain = categoryMain;
    }

    public void StoreListBtnOnClick()
    {
        print("Click success");
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;
        SceneManager.LoadScene("StoreScene");
        StoreSceneManager.categorySub = categorySub;
        StoreSceneManager.storeName = clickObj.GetComponentInChildren<TextMeshProUGUI>().text;
        StoreSceneManager.beforeScene = true;
    }
}
