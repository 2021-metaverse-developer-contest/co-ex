using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareBtnClick : MonoBehaviour
{
	public void ShareBtnOnClick()
	{
		string subject = "42_coU";
		// string scene = "StoreScene";
		string name = StoreSceneManager.storeName;
		string categoryMain = StoreSceneManager.categoryMain;
		string categorySub = StoreSceneManager.categorySub;
		// 카테고리메인에 "레스토랑&카페", 카테고리서브 "카페/디저트" &, / 쓰는 것을 조심해야함.
		// string uri = string.Format("https://exgs.github.io/yunsleeMap/urlScheme.html?scene={0}&name={1}&categoryMain={2}&categorySub={3}",
		// 					scene, name, categoryMain, categorySub);
		string uri = string.Format("https://exgs.github.io/yunsleeMap/urlScheme.html?{0},{1},{2}",
		name, categoryMain, categorySub);
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
