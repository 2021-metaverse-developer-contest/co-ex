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
        if (!Login.Instance.GetIsLogin())
        {
            GameObject.Find("Canvas_Login").transform.Find("Panel_Login").gameObject.SetActive(true);
        }
        else
        {
            //컨텐츠 업로드 페이지로 이동
        }
    }

    // login pop up
    public void PopCloseBtnOnClick()
    {
        GameObject.Find("Panel_Login").gameObject.SetActive(false);
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
        SceneManager.LoadSceneAsync("LoginScene");
        SceneManager.UnloadSceneAsync("MenuScene");
    }
}
