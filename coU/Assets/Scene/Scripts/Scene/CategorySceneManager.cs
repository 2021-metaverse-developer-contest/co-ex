using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using SQLite4Unity3d;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class CategorySceneManager : MonoBehaviour
{
    public GameObject categoryFactory;
    GameObject cvsQuit;

    // Start is called before the first frame update
    void Start()
    {
        cvsQuit = GameObject.Find("Canvas_popQuit").gameObject;
        GameObject.Find("Panel_Quit").gameObject.SetActive(false);

        string query = "Select distinct categoryMain from Stores;";
        IDbConnection dbConn = new SqliteConnection(DBConnect.GetDBFilePath());
        dbConn.Open();
        IDbCommand dbCmd = dbConn.CreateCommand();
        dbCmd.CommandText = query;
        IDataReader reader = dbCmd.ExecuteReader();
        while (reader.Read())
        {
            GameObject category = Instantiate(categoryFactory, GameObject.Find("Content").transform);
            category.GetComponentInChildren<TextMeshProUGUI>().text = reader["categoryMain"].ToString();
        }
        reader.Dispose();
        dbCmd.Dispose();
        dbConn.Dispose();

        /*
        //SQLite4Unity3d에 있는 클래스 사용할 때(db파일을 열 수 없다고 나옴->권한도 변경해윷)
        SQLiteConnection conn = new SQLiteConnection(DBConnect.GetDBFilePath());
        System.IO.Directory.CreateDirectory(DBConnect.GetDBFilePath());
        SQLiteCommand cmd = new SQLiteCommand(conn);
        cmd.CommandText = query;
        List<Stores> items = cmd.ExecuteQuery<Stores>();
        foreach (var item in items)
        {
            print(item.categoryMain);
        }
        conn.Close();
        */

        /*
        //db서버 연동시켰을 때
        SqlConnection conn = new SqlConnection(DBConnect.GetDBFilePath());
        SqlCommand cmd = new SqlCommand(query, conn);
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            print("item" + reader["categoryMain"].ToString());
            GameObject category = Instantiate(categoryFactory, GameObject.Find("Content").transform);
            category.GetComponentInChildren<Text>().text = reader["categoryMain"].ToString();
        
        }
        reader.Close();
        conn.Close();
        */

        //string dbName = "stores_v1.db";
        //string dbPath = Application.streamingAssetsPath + "/" + dbName;
        //var db = new SQLiteConnection(dbPath);
        //List<SQLiteConnection.ColumnInfo> storesDB = db.GetTableInfo("Stores");

        //var items = db.Query<Stores>("Select distinct categoryMain from Stores");
        //foreach (var item in items)
        //{
        //    print(item.categoryMain);
        //    GameObject category = Instantiate(categoryFactory, GameObject.Find("Content").transform);
        //    category.GetComponentInChildren<Text>().text = item.categoryMain;
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
        cvsQuit.transform.Find("Panel_Quit").gameObject.SetActive(true);
    }

    public static void popUpInActive()
    {
        GameObject.Find("Panel_Quit").gameObject.SetActive(false);
    }

}
