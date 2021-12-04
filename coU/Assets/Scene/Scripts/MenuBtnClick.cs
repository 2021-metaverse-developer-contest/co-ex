using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtnClick : MonoBehaviour
{
    public void CloseBtnOnClick()
    {
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
            SceneManager.UnloadSceneAsync("MenuScene");
            SceneManager.LoadSceneAsync("UploadScene", LoadSceneMode.Additive);
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
        DontDestroyManager.LoginScene.isLogin = false;
        DontDestroyManager.LoginScene.user = null;
        DontDestroyManager.LoginScene.isAdvertise = false;

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MaxstScene"))
        {
            //GameObject.Find("Panel_MenuScene").transform.Find("Panel_Login").gameObject.SetActive(true);
            //GameObject.Find("Panel_Logout").SetActive(false);
            SceneManager.UnloadSceneAsync("MenuScene");
        }
        else
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            //SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Additive);
        }
#if UNITY_EDITOR
        Debug.Log("로그아웃 되었습니다.");
#elif UNITY_ANDROID
        Toast.ShowToastMessage("로그아웃 되었습니다.", Toast.Term.shortTerm);
#endif
    }
}
