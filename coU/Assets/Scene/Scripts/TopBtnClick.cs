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

        DontDestroyManager.newPush2(DontDestroyManager.getSceneName(EventSystem.current));
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
        string beforePath = before.sceneName;

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
        SceneManager.LoadScene(before.sceneName);

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
