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
		uriString = uri.ToString();
		transName = WebUtility.UrlEncode(storeName);
		Debug.Log($"storeName {storeName}, transName {transName}");
		uriString = uriString.Replace(storeName, transName);
		Debug.Log($"URI {uriString}");
		Application.OpenURL(uriString);
	}
}
