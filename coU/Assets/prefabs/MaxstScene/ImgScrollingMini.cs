using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImgScrollingMini : MonoBehaviour
{
	public GameObject[] imgs = new GameObject[5];
	public TextMeshProUGUI tmpStoreName;
	public RectTransform content;
	int count;
	float pos;
	float movepos;
	bool isScroll = false;
	float imgWidth;
	float nextTime;
	float timeLeft = 5.0f;

	private void Start()
	{
		nextTime = Time.time + timeLeft;
		imgWidth = imgs[0].GetComponent<RectTransform>().rect.width;
		LoadImgsCoroutine();
	}

	private void Update()
	{
		if (Time.time > nextTime)
		{
			nextTime = Time.time + timeLeft;
			Right();
		}
	}

	void LoadImgsCoroutine()
	{
		StartCoroutine(LoadImgs());
	}

	IEnumerator LoadImgs()
	{
		Store store = GetDBData.getStoresData($"Select * from Stores where name = '{tmpStoreName.text}';")[0];
		WaitServer wait = new WaitServer();
		FirebaseRealtimeManager.Instance.readStoreImgs(tmpStoreName.text, wait);
		yield return wait.waitServer();
		count = FirebaseRealtimeManager.Instance.ListStoreImgs.ToArray().Length;
		int idx = count == 0 ? 1 : count;

		for (; idx < imgs.Length; idx++)
			imgs[idx].SetActive(false);
		if (count > 0)
		{
			FirebaseRealtimeManager.Instance.ListStoreImgs.Sort(StoreImg.sortOrdercmp);
			int i = 0;

			foreach (StoreImg img in FirebaseRealtimeManager.Instance.ListStoreImgs)
			{
				WaitServer wait2 = new WaitServer();
				FirebaseStorageManager.Instance.LoadFile(img, wait2);
				yield return wait2.waitServer();

				Uri uri = FirebaseStorageManager.uri;
				UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
				yield return www.SendWebRequest();
				if (www.isNetworkError || www.isHttpError)
					Debug.Log($"UnityWebRequestError: {www.error}");
				else
				{
					Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
					imgs[i++].GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
						new Vector2(.5f, .5f));
				}
			}
			movepos = imgWidth * (count - 1) / 2;
			while (Vector2.Distance(content.localPosition, new Vector2(movepos, 0)) >= 0.1f)
				content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
			pos = content.localPosition.x;
		}
		else
		{
			Texture2D texture = Resources.Load(store.logoPath, typeof(Texture2D)) as Texture2D;
			if (texture == null)
				texture = Resources.Load("default_logo", typeof(Texture2D)) as Texture2D;
			imgs[0].GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);
			Debug.Log($"storeName {store.name}");
		}
	}

	void Right()
	{
		if (count > 0)
		{
			if (content.rect.xMin + content.rect.xMax / count == Math.Round(movepos))
			{
				movepos = imgWidth * (count - 1) / 2;
				while (Vector2.Distance(content.localPosition, new Vector2(movepos, 0)) >= 0.1f)
					content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
				pos = content.localPosition.x;
			}
			else
			{
				isScroll = true;
				movepos = pos - content.rect.width / count;
				pos = movepos;
				StartCoroutine(Scroll());
			}
		}
	}

	IEnumerator Scroll()
	{
		nextTime = Time.time + timeLeft;
		while (isScroll)
		{
			content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
			if (Vector2.Distance(content.localPosition, new Vector2(movepos, 0)) < 0.1f)
			{
				Debug.Log($"Start's Coroutine {content.localPosition}");
				isScroll = false;
			}
			yield return null;
		}
	}
}
