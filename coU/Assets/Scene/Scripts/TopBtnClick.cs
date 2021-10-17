using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopBtnClick : MonoBehaviour
{
    public void HomeBtnOnClick()
    {
        SceneManager.LoadScene("MaxstScene");
    }

    public void SearchBtnOnClick()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "StoreScene" && StoreSceneManager.beforeScene)
            SearchSceneManager.beforeItem = GameObject.Find("TMP_Name").GetComponent<TextMeshProUGUI>().text;
        else if (currentScene.name != "StoreScene")
            SearchSceneManager.beforeItem = "";
        SearchSceneManager.beforeScene = currentScene.buildIndex;
        SceneManager.LoadScene("SearchScene");
    }

    public void BackBtnOnClick()
    {
        string activeScene = SceneManager.GetActiveScene().name;

        if (activeScene.Contains("Search"))
        {
            if (SearchSceneManager.beforeItem != ""
                && SceneUtility.GetScenePathByBuildIndex(SearchSceneManager.beforeScene).Contains("StoreScene")) 
            {
                StoreSceneManager.beforeScene = true;
                StoreSceneManager.storeName = SearchSceneManager.beforeItem;
            }
            SearchSceneManager.searchStr = "";
            SceneManager.LoadScene(SearchSceneManager.beforeScene);
        }
        else if (activeScene.Contains("StoreList"))
            SceneManager.LoadScene("AllCategoryScene");
        else if (activeScene.Contains("Store"))
        {
            if (StoreSceneManager.beforeScene)
                SceneManager.LoadScene("StoreListScene");
            else
            {
                SearchSceneManager.beforeScene = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene("SearchScene");
            }
        }
    }

}
