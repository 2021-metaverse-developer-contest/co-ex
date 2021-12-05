using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
            DontDestroyManager.newPush(sceneName_: DontDestroyManager.getSceneName(EventSystem.current), storeName_: DontDestroyManager.StoreScene.storeName, categorySub_: DontDestroyManager.StoreScene.categorySub);
        else if (currentScene.name.Contains("StoreListScene"))
            DontDestroyManager.newPush(sceneName_: DontDestroyManager.getSceneName(EventSystem.current), categorySub_: DontDestroyManager.StoreListScene.categorySub);
        else
            DontDestroyManager.newPush(sceneName_: DontDestroyManager.getSceneName(EventSystem.current));
        DontDestroyManager.SearchScene.searchStr = "";
        SceneManager.LoadScene("SearchScene");
    }

    public void BackBtnOnClick()
    {
        string curScene = SceneManager.GetActiveScene().name;
        if (Stack.Instance.Count() == 0)
        {
            SceneManager.LoadScene("AllCategoryScene");
            return;
        }
        SceneInfo before = Stack.Instance.Pop();
        string beforePath = SceneUtility.GetScenePathByBuildIndex(before.beforeScene);

        //if (curScene.Contains("StoreScene") && beforePath.Contains("MaxstScene"))
        if (curScene.Contains("MaxstScene") && beforePath.Contains("MaxstScene"))
        {
            Stack.Instance.Clear();
            SceneManager.UnloadScene("StoreScene");
            return;
        }
        
        if (beforePath.Contains("StoreScene"))
        {
            DontDestroyManager.StoreScene.storeName = before.storeName;
            DontDestroyManager.StoreScene.categorySub = before.categorySub;
        }
        else if (beforePath.Contains("StoreListScene"))
            DontDestroyManager.StoreListScene.categorySub = before.categorySub;
        else if (beforePath.Contains("SearchScene"))
            DontDestroyManager.SearchScene.searchStr = before.storeName;
        else //MaxstScene으로 가던, AllCategoryScene으로 가던 스택 비워줘야 함.
            Stack.Instance.Clear();
        SceneManager.LoadScene(before.beforeScene);

    }

    public void ARBtnOnClick()
    {
        Stack.Instance.Clear();
        SceneManager.LoadScene("MaxstScene");
    }

    public void MenuBtnOnclick()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Additive);
    }
}
