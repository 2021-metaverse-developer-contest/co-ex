using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TopBtnClick : MonoBehaviour
{
    /// <summary>
	/// MaxstScene에는 홈버튼이 존재하지 않기 때문에 MaxstScene에서 눌렸는지 확인해줄 필요 없음
	/// 홈으로 가면 뒤로가기도 무조건 맥스트씬이기 때문에 Stack을 Clear함.
	/// </summary>
    public void HomeBtnOnClick()
    {
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;
        Stack.Instance.Clear();
        SceneManager.LoadSceneAsync("AllCategoryScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(clickObj.scene);
    }

    public void SearchBtnOnClick()
    {
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;
        string curScene = clickObj.scene.name;
        //Scene currentScene = SceneManager.GetActiveScene();

        if (curScene.Contains("StoreScene"))
            DontDestroyManager.newPush(sceneName_: DontDestroyManager.getSceneName(EventSystem.current), storeName_: DontDestroyManager.StoreScene.storeName, categorySub_: DontDestroyManager.StoreScene.categorySub);
        else if (curScene.Contains("StoreListScene"))
            DontDestroyManager.newPush(sceneName_: DontDestroyManager.getSceneName(EventSystem.current), categorySub_: DontDestroyManager.StoreListScene.categorySub);
        else if (curScene.Contains("MaxstScene"))
            GameObject.Find("Canvas_Parent").SetActive(false);
        else if (!curScene.Contains("MenuScene")) //MenuScene은 스택에 넣지 않음
            DontDestroyManager.newPush(sceneName_: DontDestroyManager.getSceneName(EventSystem.current));
        DontDestroyManager.SearchScene.searchStr = "";
        SceneManager.LoadSceneAsync("SearchScene", LoadSceneMode.Additive);
        if (!curScene.Contains("MaxstScene"))
            SceneManager.UnloadSceneAsync(curScene);
    }

    public void BackBtnOnClick()
    {
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;
        string curScene = clickObj.scene.name;

        Debug.Log($"Stack count {Stack.Instance.Count()}");
        //string curScene = SceneManager.GetActiveScene().name;
        if (Stack.Instance.Count() == 0) //Stack이 비워져있다는 얘기는 무조건 UnloadScene
        {
            GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var obj in gameObjects)
                if (obj.name == "Canvas_Parent")
                    obj.SetActive(true);
            
            SceneManager.UnloadSceneAsync(curScene);
            //SceneManager.LoadScene("AllCategoryScene");
            return;
        }
        SceneInfo before = Stack.Instance.Pop();
        string beforePath = SceneUtility.GetScenePathByBuildIndex(before.beforeScene);

        /*
         * hyojlee 2021/12/5
         * MaxstScene을 항상 active하게 함으로써 Stack의 count가 0인 경우에는 위에서의 처리로 해결됨
         * 즉, 아래의 코드는 MaxstScene에서 매장 프리뷰를 클릭했을 경우를 의미하므로 굳이 필요하지 않음
          
        //if (curScene.Contains("StoreScene") && beforePath.Contains("MaxstScene"))
        if (curScene.Contains("MaxstScene") && beforePath.Contains("MaxstScene"))
        {
            Stack.Instance.Clear();
            SceneManager.UnloadSceneAsync("StoreScene");
            return;
        }
         */

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
        SceneManager.LoadSceneAsync(before.beforeScene, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(curScene);
    }

    public void ARBtnOnClick()
    {
        Stack.Instance.Clear();
        GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in gameObjects)
            if (obj.name == "Canvas_Parent")
                obj.SetActive(true);
        SceneManager.UnloadSceneAsync("MenuScene"); //공간 인식 버튼은 메뉴씬에서 누를 수 밖에 없기 때문
        //SceneManager.LoadSceneAsync("MaxstScene", LoadSceneMode.Single);
    }

    public void MenuBtnOnclick()
    {
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;
        string curScene = clickObj.scene.name;

        if (curScene.Contains("StoreScene"))
            DontDestroyManager.newPush(sceneName_: DontDestroyManager.getSceneName(EventSystem.current), storeName_: DontDestroyManager.StoreScene.storeName, categorySub_: DontDestroyManager.StoreScene.categorySub);
        else if (curScene.Contains("StoreListScene"))
            DontDestroyManager.newPush(sceneName_: DontDestroyManager.getSceneName(EventSystem.current), categorySub_: DontDestroyManager.StoreListScene.categorySub);
        else if (curScene.Contains("SearchScene"))
            DontDestroyManager.newPush(sceneName_: DontDestroyManager.getSceneName(EventSystem.current), storeName_: DontDestroyManager.SearchScene.searchStr);
        else if (curScene.Contains("MaxstScene")) //MaxstScene은 스택에 넣지 않음
            GameObject.Find("Canvas_Parent").SetActive(false);
        else //MaxstScene은 스택에 넣지 않음
            DontDestroyManager.newPush(sceneName_: DontDestroyManager.getSceneName(EventSystem.current));
        SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Additive);
        if (!curScene.Contains("MaxstScene"))
            SceneManager.UnloadSceneAsync(curScene);
    }
}
