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
        List<Store> stores = GetDBData.getStoresData(query);
        foreach (Store store in stores)
        {
            GameObject categorySub = Instantiate(subCtFactory, GameObject.Find("Content").transform);
            categorySub.GetComponentInChildren<TextMeshProUGUI>().text = store.categorySub;
        }
        print("Done SubCategory");
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
