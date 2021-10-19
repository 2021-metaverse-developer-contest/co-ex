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
		string scene = "StoreScene";
		string name = StoreSceneManager.storeName;
        string categoryMain = StoreSceneManager.categoryMain;
        string categorySub = StoreSceneManager.categorySub;

    string uri = string.Format("https://github.com/exgs/yunsleeMap?scene={0}&name={1}&categoryMain={2}&categorySub={3}",
                            scene, name, categoryMain, categorySub);
        #if UNITY_ANDROID && !UNITY_EDITOR
		    AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		    AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
		    intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		    intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		    intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		    intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
		    AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		    AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		    AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
		    currentActivity.Call("startActivity", jChooser);
        #endif
    }

    public void NaviBtnOnClick()
    {
        SceneManager.LoadScene("MaxstScene");

        MaxstSceneManager.naviStoreName = StoreSceneManager.storeName;
        MaxstSceneManager.naviStoreCategorySub = StoreSceneManager.categorySub;
        MaxstSceneManager.naviStoreFloor = StoreSceneManager.floor;

        MaxstSceneManager.StartNavigation(StoreSceneManager.storeName, StoreSceneManager.categorySub, StoreSceneManager.floor);
    }
}
