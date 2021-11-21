using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.IO;

public class GetDBData : MonoBehaviour
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

    public static List<Store> getStoresData(string query)
    {
        string persistentDBTotalPath = Path.Combine(Application.persistentDataPath, dbName);
        SQLiteConnection db = new SQLiteConnection(persistentDBTotalPath);
        List<Store> getClass = db.Query<Store>(query);
        db.Dispose();
        return getClass;
    }

    public static List<Item> getItemsData(string query)
    {
        string persistentDBTotalPath = Path.Combine(Application.persistentDataPath, dbName);
        SQLiteConnection db = new SQLiteConnection(persistentDBTotalPath);
        List<Item> getClass = db.Query<Item>(query);
        db.Dispose();
        return getClass;
    }

    public static List<Facility> getFacilitiesData(string query)
    {
        string persistentDBTotalPath = Path.Combine(Application.persistentDataPath, dbName);
        SQLiteConnection db = new SQLiteConnection(persistentDBTotalPath);
        List<Facility> getClass = db.Query<Facility>(query);
        db.Dispose();
        return getClass;
    }
}


