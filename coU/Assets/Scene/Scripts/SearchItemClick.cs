using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SearchItemClick : MonoBehaviour
{
    public void ResultItemOnClick()
    {
        GameObject cur = EventSystem.current.currentSelectedGameObject;
        
        SceneManager.LoadScene("StoreScene");
        StoreSceneManager.storeName = cur.transform.Find("TMP_Result").GetComponent<TextMeshProUGUI>().text;
        //StoreSceneManager.categorySub = "";
        StoreSceneManager.beforeScene = false;
        StoreSceneManager.searchStr = SearchSceneManager.searchStr;
        StoreSceneManager.isMaxst = false;
    }

    public void ListItemOnClick()
    {
        GameObject cur = EventSystem.current.currentSelectedGameObject;

        SceneManager.LoadScene("StoreScene");
        StoreSceneManager.storeName = cur.transform.Find("TMP_List").GetComponent<TextMeshProUGUI>().text;
        //StoreSceneManager.categorySub = "";
        StoreSceneManager.beforeScene = false;
        StoreSceneManager.searchStr = StoreSceneManager.storeName;
        StoreSceneManager.isMaxst = false;
    }
}