using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Net;

public class ImgClick : MonoBehaviour, IPointerClickHandler
{
	public Uri uri;
	public string storeName;
	private string transName;
	private string uriString;

	public void OnPointerClick(PointerEventData eventData)
	{
		try
		{
			uriString = uri.ToString();
			transName = WebUtility.UrlEncode(storeName);
			Debug.Log($"storeName {storeName}, transName {transName}");
			uriString = uriString.Replace(storeName, transName);
			Debug.Log($"URI {uriString}");
#if UNITY_EDITOR
			Application.OpenURL(uriString);
#elif !UNITY_EDITOR && UNITY_ANDROID
			WebView temp = new WebView(storeName, uriString);
			temp.WebViewUnity();
#endif
		}
		catch (Exception e)
		{
			Debug.LogError($"Image Click Error: {e.StackTrace}");
		}
	}
}
