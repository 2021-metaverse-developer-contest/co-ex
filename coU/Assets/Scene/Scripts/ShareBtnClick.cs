using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Net;

public class ShareBtnClick : MonoBehaviour
{
	public void ShareBtnOnClick()
	{
		string subject = "co-ex 애플리케이션에서 상대방의 위치와 가까운 매장정보를 확인하시겠습니까? 링크를 눌러주세요!";
		string name;
		string categoryMain;
		string categorySub;
		string sceneName = SceneManager.GetActiveScene().name;
		if (sceneName.Contains("StoreScene") || (SceneManager.sceneCount > 1 && sceneName.Contains("MaxstScene")))
		{
			name = DontDestroyManager.StoreScene.storeName;
			categoryMain = DontDestroyManager.StoreScene.categoryMain;
			categorySub = DontDestroyManager.StoreScene.categorySub;
		}
		else
		{
			name = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<TextMeshProUGUI>().text;
			List<Store> stores = GetDBData.getStoresData("Select * from Stores where name = '" + name + "'");
			categoryMain = stores[0].categoryMain;
			categorySub = stores[0].categorySub;
		}
		// 카테고리메인에 "레스토랑&카페", 카테고리서브 "카페/디저트" &, / 쓰는 것을 조심해야함.
		// string uri = string.Format("https://exgs.github.io/yunsleeMap/urlScheme.html?scene={0}&name={1}&categoryMain={2}&categorySub={3}",
		// 					scene, name, categoryMain, categorySub);

		name = WebUtility.UrlEncode(name);
		categoryMain = WebUtility.UrlEncode(categoryMain);
		categorySub = WebUtility.UrlEncode(categorySub);
		string uri = string.Format("https://exgs.github.io/yunsleeMap/urlScheme.html?parameter={0},{1},{2}",
		name, categoryMain, categorySub).Replace(" ","%20");
		print(subject);
		print(uri);
#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), uri);
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
		currentActivity.Call("startActivity", jChooser);
#elif UNITY_IOS && !UNITY_EDITOR
		Application.OpenURL(uri);
#elif UNITY_EDITOR
		Application.OpenURL(uri);
#endif
		//Application.OpenURL("https://www.naver.com");
	}
}
