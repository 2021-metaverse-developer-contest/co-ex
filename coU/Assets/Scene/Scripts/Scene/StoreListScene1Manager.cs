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
        List<Store> stores = GetDBData.getStoresData(query);
        foreach(Store store in stores)
        {
            categoryMain = store.categoryMain;
            GameObject name = Instantiate(storeFactory, GameObject.Find("Content").transform);
            name.GetComponentInChildren<TextMeshProUGUI>().text = store.name;
        }
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
