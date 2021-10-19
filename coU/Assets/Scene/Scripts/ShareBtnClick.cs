using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareBtnClick : MonoBehaviour
{
	public void ShareBtnOnClick()
	{
		string subject = "42_coU";
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
		    intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), uri);
		    AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		    AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		    AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
		    currentActivity.Call("startActivity", jChooser);
#endif
	}
}
