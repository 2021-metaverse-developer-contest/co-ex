using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using SQLite4Unity3d;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SubCtSceneManager : MonoBehaviour
{
    public GameObject subCtFactory;
    public static string categoryMain;
    int numOfSubCt = 0;

    // Start is called before the first frame update
    void Start()
    {
        print("categoryMain" + categoryMain);

        string query = "Select distinct categorySub from Stores where categoryMain = '" + categoryMain + "'";
        IDbConnection dbConn = new SqliteConnection(DBConnect.GetDBFilePath());
        dbConn.Open();
        IDbCommand dbCmd = dbConn.CreateCommand();
        dbCmd.CommandText = query;
        IDataReader reader = dbCmd.ExecuteReader();
        while (reader.Read())
        {
            GameObject categorySub = Instantiate(subCtFactory, GameObject.Find("Content").transform);
            categorySub.GetComponentInChildren<TextMeshProUGUI>().text = reader["categorySub"].ToString();
        }
        reader.Dispose();
        dbCmd.Dispose();
        dbConn.Dispose();
        print("Done SubCategory");
        //string dbPath = Application.streamingAssetsPath + "/stores_v1.db";
        //var db = new SQLiteConnection(dbPath);
        //List<SQLiteConnection.ColumnInfo> storesDB = db.GetTableInfo("Stores");

        //var items = db.Query<Stores>("Select distinct categorySub from Stores where categoryMain = '" + categoryMain + "'");
        //numOfSubCt = items.Count;
        //print("numOfSubCt = " + numOfSubCt.ToString());
        //foreach (var item in items)
        //{
        //    print(item.categorySub);
        //    GameObject categorySub = Instantiate(subCtFactory, GameObject.Find("Content").transform);
        //    categorySub.GetComponentInChildren<Text>().text = item.categorySub;
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
        SceneManager.LoadScene("CategoryScene");
    }
}
