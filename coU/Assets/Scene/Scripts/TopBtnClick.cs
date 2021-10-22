using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopBtnClick : MonoBehaviour
{
    public void HomeBtnOnClick()
    {
        Stack.Instance.Clear();
        SceneManager.LoadScene("AllCategoryScene");
    }

    public void SearchBtnOnClick()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneInfo curInfo;

        if (currentScene.name.Contains("StoreScene"))
            curInfo = new SceneInfo(currentScene.buildIndex, StoreSceneManager.storeName, StoreSceneManager.categorySub);
        else if (currentScene.name.Contains("StoreListScene"))
            curInfo = new SceneInfo(currentScene.buildIndex, StoreListSceneManager.categorySub, false);
        else
            curInfo = new SceneInfo(currentScene.buildIndex);
        Stack.Instance.Push(curInfo);
        SearchSceneManager.searchStr = "";
        SceneManager.LoadScene("SearchScene");
    }

    public void BackBtnOnClick()
    {
        string curScene = SceneManager.GetActiveScene().name;
        SceneInfo before = Stack.Instance.Pop();
        string beforePath = SceneUtility.GetScenePathByBuildIndex(before.beforeScene);

        //if (curScene.Contains("StoreScene") && beforePath.Contains("MaxstScene"))
        if (curScene.Contains("MaxstScene") && beforePath.Contains("MaxstScene"))
        {
            Stack.Instance.Clear();
            SceneManager.UnloadSceneAsync("StoreScene");
             return;
        }
        
        if (beforePath.Contains("StoreScene"))
        {
            StoreSceneManager.storeName = before.storeName;
            StoreSceneManager.categorySub = before.categorySub;
        }
        else if (beforePath.Contains("StoreListScene"))
            StoreListSceneManager.categorySub = before.categorySub;
        else if (beforePath.Contains("SearchScene"))
            SearchSceneManager.searchStr = before.storeName;
        else //MaxstScene으로 가던, AllCategoryScene으로 가던 스택 비워줘야 함.
            Stack.Instance.Clear();
        SceneManager.LoadScene(before.beforeScene);

    }

    public void ARBtnOnClick()
    {
        Stack.Instance.Clear();
        SceneManager.LoadScene("MaxstScene");
    }
}
