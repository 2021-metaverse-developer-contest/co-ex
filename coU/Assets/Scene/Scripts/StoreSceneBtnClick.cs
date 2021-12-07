using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StoreSceneBtnClick : MonoBehaviour
{
    public void MapBtnOnClick()
    {
        string query = "Select * from Stores Where name = '"
                        + GameObject.Find("TMP_Name").GetComponent<TextMeshProUGUI>().text + "'";
        List<Store> store = GetDBData.getStoresData(query);
        string mapLink = "https://m.starfield.co.kr/coexmall/tenant/tenantDetail/"
                        + store[0].tntSeq + "?maps=" + store[0].mapKey;
        Debug.Log("mapLink " + store[0].mapKey);
#if UNITY_EDITOR
        Application.OpenURL(mapLink);
#elif !UNITY_EDITOR && UNITY_ANDROID
        WebView temp = new WebView(storeName: store[0].name, url: mapLink);
        temp.WebViewUnity();
#else
        Application.OpenURL(mapLink);
#endif
    }

    public void CallBtnOnClick()
    {
        string phone = GameObject.Find("TMP_Phone").GetComponent<TextMeshProUGUI>().text;
        Application.OpenURL("tel:" + phone);
    }

    public void UploadBtnOnClick()
    {
        Scene curScene = EventSystem.current.currentSelectedGameObject.scene;
        DontDestroyManager.newPush(curScene.name, DontDestroyManager.StoreScene.storeName, DontDestroyManager.StoreScene.categorySub);
        SceneManager.LoadSceneAsync("UploadScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(curScene);
        DontDestroyManager.UploadScene.isBeforeMenu = false;
    }

    public void NaviCharacterPopUp()
    {
        GameObject.Find("Canvas_Choose").transform.Find("Panel_ChooseWhole").gameObject.SetActive(true);
    }

    public void NaviBtnOnClick()
    {
        string charac = GameObject.Find("TMP_Choose").GetComponent<TextMeshProUGUI>().text;
        if (charac == "선택안함")
            NavigationController.characterType = NavigationController.e_character.none;
        else if (charac == "우주인")
            NavigationController.characterType = NavigationController.e_character.astronaut;
        else if (charac == "토끼")
            NavigationController.characterType = NavigationController.e_character.rabbit;
        else
            NavigationController.characterType = NavigationController.e_character.coco;


        Debug.Log("Navigation Btn click " + DontDestroyManager.StoreScene.floor);
        DontDestroyManager.MaxstScene.naviStoreName = DontDestroyManager.StoreScene.storeName;
        DontDestroyManager.MaxstScene.naviStoreCategorySub = DontDestroyManager.StoreScene.categorySub;
        DontDestroyManager.MaxstScene.naviStoreFloor = DontDestroyManager.StoreScene.floor;
        DontDestroyManager.MaxstScene.chkNaviBtnClick = true;
        //Stack.Instance.Push(new SceneInfo(SceneManager.GetActiveScene().buildIndex, DontDestroyManager.DontDestroyManager.StoreScene.storeName, StoreSceneManager.categorySub));
        Stack.Instance.Clear(); // MaxstScene에서는 언제나 바로 종료되므로
        SceneManager.UnloadSceneAsync("StoreScene");

        GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in gameObjects)
            if (obj.name == "Canvas_Parent")
                obj.SetActive(true);

        //      if (SceneManager.sceneCount == 1)
        //{
        //          SceneManager.LoadSceneAsync("MaxstScene", LoadSceneMode.Single);
        //}
        //      else
        //{
        //          SceneManager.UnloadSceneAsync("StoreScene");
        //      }
    }

    public void NaviPopCloseBtnOnClick()
    {
        GameObject.Find("Panel_ChooseWhole").SetActive(false);
    }
}
