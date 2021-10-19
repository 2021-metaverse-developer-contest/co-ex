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

    public void ShareBtnOnClick()
    {

    }

    public void NaviBtnOnClick()
    {

        MaxstSceneManager.naviStoreName = StoreSceneManager.storeName;
        MaxstSceneManager.naviStoreCategorySub = StoreSceneManager.categorySub;
        MaxstSceneManager.naviStoreFloor = StoreSceneManager.floor;

        MaxstSceneManager.StartNavigation(StoreSceneManager.storeName, StoreSceneManager.categorySub, StoreSceneManager.floor);
        MaxstSceneManager.isStart = false;
        SceneManager.LoadScene("MaxstScene");
    }
}
