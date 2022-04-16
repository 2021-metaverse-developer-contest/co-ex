using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm;
using Gpm.WebView;

public class WebView
{
	string url;
	string storeName;
	public WebView(string storeName, string url)
	{
		this.storeName = storeName;
		this.url = url;
	}

	public void WebViewUnity()
	{
		GpmWebView.ShowUrl(url,
					new GpmWebViewRequest.Configuration()
					{
						style = GpmWebViewStyle.FULLSCREEN,
						isClearCookie = false,
						isClearCache = false,
						isNavigationBarVisible = true,
						title = storeName,
						isBackButtonVisible = false,
						isForwardButtonVisible = false,
#if UNITY_IOS
                contentMode = GpmWebViewContentMode.MOBILE
#endif
					},
		OnOpenCallback,
		OnCloseCallback,
		new List<string>()
		{
			"USER_CUSTOM_SCHEME"
		},
		OnSchemeEvent);
	}

	private void OnOpenCallback(GpmWebViewError error)
	{
		if (error == null)
		{
			Debug.Log("[OnOpenCallback] succeeded.");
		}
		else
		{
			Debug.Log(string.Format("[OnOpenCallback] failed. error:{0}", error));
		}
	}

	private void OnCloseCallback(GpmWebViewError error)
	{
		if (error == null)
		{
			Debug.Log("[OnCloseCallback] succeeded.");
		}
		else
		{
			Debug.Log(string.Format("[OnCloseCallback] failed. error:{0}", error));
		}
	}

	private void OnSchemeEvent(string data, GpmWebViewError error)
	{
		if (error == null)
		{
			Debug.Log("[OnSchemeEvent] succeeded.");

			if (data.Equals("USER_ CUSTOM_SCHEME") == true || data.Contains("CUSTOM_SCHEME") == true)
			{
				Debug.Log(string.Format("scheme:{0}", data));
			}
		}
		else
		{
			Debug.Log(string.Format("[OnSchemeEvent] failed. error:{0}", error));
		}
	}
}
