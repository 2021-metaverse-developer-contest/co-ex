using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using SQLite4Unity3d;
using UnityEngine;
using UnityEngine.Networking;

public class DBConnect : MonoBehaviour
{
    public static string GetDBFilePath() //파일이 생긴 경로를 얻어오는 함수
    {
        string str;
        string dbName = "Starfield.db";

        if (Application.platform == RuntimePlatform.Android)
        {
            str = "URI=file:" + Application.persistentDataPath + "/" + dbName;//IOS는 Data Source=로 시작한다고 함.
        }
        else
        {
            str = "URI=file:" + Application.dataPath + "/" + dbName;
            print("dbName " + dbName);
            //str = "Data Source=" + Application.dataPath + "/" + dbName;
        }
        return str;
    }
    public static List<Stores> CustomExcuteQuery(string query)
    {
        List<Stores> stores = new List<Stores>();
        IDbConnection conn = new SqliteConnection(GetDBFilePath());
        conn.Open();
        IDbCommand cmd = conn.CreateCommand();
        cmd.CommandText = query;
        IDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Stores temp = new Stores();
            temp.index = Int32.Parse(reader["index"].ToString());
            temp.name = reader["name"].ToString();
            temp.floor = reader["floor"].ToString();
            temp.phoneNumber = reader["phoneNumber"].ToString();
            temp.openHour = reader["openHour"].ToString();
            temp.categoryMain = reader["categoryMain"].ToString();
            temp.categorySub = reader["categorySub"].ToString();
            temp.logoPath = reader["logoPath"].ToString();
            temp.tntSeq = reader["tntSeq"].ToString();

            // 언더아머 매장의 x,y 좌표가 없어서 null로 저장되어있어서 예외처리가 필요함.
            if (reader["x"] == System.DBNull.Value || reader["y"] == System.DBNull.Value)
            {
                temp.x = 0f;
                temp.y = 0f;
                print((reader["x"]).GetType()); // System.DBNull
            }
            else
            {
                temp.x = float.Parse(reader["x"].ToString());
                temp.y = float.Parse(reader["y"].ToString());
            }
            stores.Add(temp);
        }
        reader.Dispose();
        cmd.Dispose();
        conn.Dispose();
        return stores;
    }
}
