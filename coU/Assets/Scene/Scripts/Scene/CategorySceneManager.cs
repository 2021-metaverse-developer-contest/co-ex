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
        List<Store>stores= GetDBData.getStoresData(query);

        foreach (Store store in stores)
        {
            GameObject category = Instantiate(categoryFactory, GameObject.Find("Content").transform);
            category.GetComponentInChildren<TextMeshProUGUI>().text = store.categoryMain;
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
        cvsQuit.transform.Find("Panel_Quit").gameObject.SetActive(true);
    }

    public static void popUpInActive()
    {
        GameObject.Find("Panel_Quit").gameObject.SetActive(false);
    }

}
