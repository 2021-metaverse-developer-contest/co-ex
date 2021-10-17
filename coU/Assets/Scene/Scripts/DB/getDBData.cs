using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.IO;

public class getDBData : MonoBehaviour
{
    public static string dbName = "Starfield.db";
    public static List<Total> getClassData(string query)
    {
        string persistentDBTotalPath = Path.Combine(Application.persistentDataPath, dbName);
        SQLiteConnection db = new SQLiteConnection(persistentDBTotalPath);
        List<Total> getClass = db.Query<Total>(query);
        db.Dispose();
        return getClass;
    }

    public static List<Category> getCategoryData(string query)
    {
        string persistentDBTotalPath = Path.Combine(Application.persistentDataPath, dbName);
        SQLiteConnection db = new SQLiteConnection(persistentDBTotalPath);
        List<Category> getClass = db.Query<Category>(query);
        db.Dispose();
        return getClass;

    }

    public static List<Stores> getStoresData(string query)
    {
        string persistentDBTotalPath = Path.Combine(Application.persistentDataPath, dbName);
        SQLiteConnection db = new SQLiteConnection(persistentDBTotalPath);
        List<Stores> getClass = db.Query<Stores>(query);
        db.Dispose();
        return getClass;
    }

    public static List<Items> getItemsData(string query)
    {
        string persistentDBTotalPath = Path.Combine(Application.persistentDataPath, dbName);
        SQLiteConnection db = new SQLiteConnection(persistentDBTotalPath);
        List<Items> getClass = db.Query<Items>(query);
        db.Dispose();
        return getClass;
    }
}


