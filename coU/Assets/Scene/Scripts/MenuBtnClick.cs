using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuBtnClick : MonoBehaviour
{
    public void CloseBtnOnClick()
    {
        if (Stack.Instance.Count() == 0)
        {
            GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var obj in gameObjects)
                if (obj.name == "Canvas_Parent")
                    obj.SetActive(true);
        }
        else
        {
            SceneInfo before = Stack.Instance.Pop();
            string beforePath = SceneUtility.GetScenePathByBuildIndex(before.beforeScene);
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
        }
        SceneManager.UnloadSceneAsync("MenuScene");
    }

    public void AdvertiseBtnOnClick()
    {
        Debug.Log("AdvertiseButton Click");
        DontDestroyManager.LoginScene.isAdvertise = true;

        //if (!Login.Instance.GetIsLogin())
        if (!DontDestroyManager.LoginScene.isLogin)
            GameObject.Find("Canvas_Pop").transform.Find("Panel_PopWhole").gameObject.SetActive(true);
        else
        {
            SceneManager.LoadSceneAsync("UploadScene", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("MenuScene");
            DontDestroyManager.UploadScene.isBeforeMenu = true;
            //컨텐츠 업로드 페이지로 이동
        }
    }

    // login pop up
    public void PopCloseBtnOnClick()
    {
        GameObject.Find("Panel_PopWhole").gameObject.SetActive(false);
        //GameObject[] gameObjects = SceneManager.GetSceneByName("MenuScene").GetRootGameObjects();

        //foreach (GameObject gameObject in gameObjects)
        //{
        //    if (gameObject.name == "Canvas_Login")
        //    {
        //        gameObject.transform.Find("Panel_Login").gameObject.SetActive(false);
        //        break;
        //    }
        //}
    }

    public void LoginBtnOnClick()
    {
        //Stack.Instance.Push(new SceneInfo(SceneManager.GetActiveScene().buildIndex));
        //SceneManager.LoadSceneAsync("LoginScene");

        // 2021.11.10
        // 로그인 씬으로 이동 시 메뉴 씬은 언로드하고 로그인 씬을 Additive모드로 로드하고자함.
        SceneManager.LoadSceneAsync("LoginScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("MenuScene");
    }

    public void LogoutBtnOnClick()
    {
        Scene curScene = EventSystem.current.currentSelectedGameObject.scene;
        DontDestroyManager.LoginScene.isLogin = false;
        DontDestroyManager.LoginScene.user = null;
        DontDestroyManager.LoginScene.isAdvertise = false;

        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MaxstScene"))
        //{
        //    //GameObject.Find("Panel_MenuScene").transform.Find("Panel_Login").gameObject.SetActive(true);
        //    //GameObject.Find("Panel_Logout").SetActive(false);
        //    SceneManager.UnloadScene("MenuScene");
        //}
        //else
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //    //SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Additive);
        //}

        GameObject panelLoginParent = GameObject.Find("Panel_MenuScene");
        GameObject.Find("MenuSceneManager").GetComponent<MenuSceneManager>().ChangeLogin(panelLoginParent);
        //SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Additive);
        //SceneManager.UnloadSceneAsync(curScene);
#if UNITY_EDITOR
        Debug.Log("로그아웃 되었습니다.");
#elif UNITY_ANDROID
        Toast.ShowToastMessage("로그아웃 되었습니다.", Toast.Term.shortTerm);
#endif
    }
}
