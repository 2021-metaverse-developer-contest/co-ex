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
        Application.OpenURL(mapLink);

    }

    public void CallBtnOnClick()
    {
        string phone = GameObject.Find("TMP_Phone").GetComponent<TextMeshProUGUI>().text;
        Application.OpenURL("tel:" + phone);
    }

    public void UploadBtnOnClick()
    {
        SceneManager.LoadSceneAsync("UploadScene", LoadSceneMode.Additive);
        UploadSceneManager.isBeforeMenu = false;
    }

    public void NaviCharacterPopUp()
    {
        GameObject.Find("Canvas_Choose").transform.Find("Panel_ChooseWhole").gameObject.SetActive(true);
    }

    public void NaviBtnOnClick()
    {
        string charac = GameObject.Find("TMP_Choose").GetComponent<TextMeshProUGUI>().text;
        if (charac == "선택안함")
            NavigationController.character = NavigationController.e_character.none;
        else if (charac == "우주인")
            NavigationController.character = NavigationController.e_character.astronaut;
        else if (charac == "토끼")
            NavigationController.character = NavigationController.e_character.rabbit;
        else
            NavigationController.character = NavigationController.e_character.coco;


        Debug.Log("Navigation Btn click " + StoreSceneManager.floor);
        MaxstSceneManager.naviStoreName = StoreSceneManager.storeName;
        MaxstSceneManager.naviStoreCategorySub = StoreSceneManager.categorySub;
        MaxstSceneManager.naviStoreFloor = StoreSceneManager.floor;
        MaxstSceneManager.chkNaviBtnClick = true;
        //Stack.Instance.Push(new SceneInfo(SceneManager.GetActiveScene().buildIndex, StoreSceneManager.storeName, StoreSceneManager.categorySub));
        Stack.Instance.Clear(); // MaxstScene에서는 언제나 바로 종료되므로
        SceneManager.LoadScene("MaxstScene");
    }

    public void NaviPopCloseBtnOnClick()
    {
        GameObject.Find("Panel_ChooseWhole").SetActive(false);
    }
}
