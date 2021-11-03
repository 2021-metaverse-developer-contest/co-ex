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
        Stack.Instance.Push(new SceneInfo(SceneManager.GetActiveScene().buildIndex));

        SceneManager.LoadSceneAsync("LoginScene");
    }
}
