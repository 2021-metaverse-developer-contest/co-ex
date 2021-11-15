using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEngine.Networking;

public class UploadSceneManager : MonoBehaviour
{
	[SerializeField]
	private GameObject item;
	Transform itemParent;
	public static bool isBeforeMenu = false;

	public static List<StoreImg> ListStoreImgs;
	public string storeName;
	public string imgType;

	private void Awake()
	{
		if (ListStoreImgs == null)
			ListStoreImgs = new List<StoreImg>();
		ListStoreImgs.Clear();
		this.storeName = LoginSceneManager.user?.storeName;
		Debug.Log("UploadScene StoreName " + storeName);
	}

	private void Start()
	{
		Screen.orientation = ScreenOrientation.Portrait;
		GameObject.Find("TMP_StoreName").GetComponent<TextMeshProUGUI>().text = storeName;
		//LoadCoroutine();
		itemParent = GameObject.Find("ContentUpload").transform;
		ListStoreImgs.Clear();
		init();
	}

	private void Update()
	{
	}

	public void getDataCoroutine()
	{
		StartCoroutine(getData());
	}

	//public void downloadCoroutine()
	//{
	//	destFullPath = "/Users/yunslee/desttest.png";
	//	StartCoroutine(Download());
	//}

	public void LoadCoroutine(string imgPath)
	{
		StartCoroutine(Load(imgPath));
	}

	//public IEnumerator Download()
	//{
	//	StoreImg data = new StoreImg(storeName, imgType, sortOrder);
	//	FirebaseStorageManager.Instance.downloadFile(data, destFullPath);
	//	yield return WaitServer.Instance.waitServer();
	//}

	IEnumerator Load(string imgPath)
	{
		StoreImg data = new StoreImg(imgPath);
		FirebaseStorageManager.Instance.LoadFile(data);
		yield return WaitServer.Instance.waitServer();
		Uri uri = FirebaseStorageManager.uri;
		Image img = GetUIImage();
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError)
			Debug.Log(www.error);
		else
		{
			Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
			img.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(.5f, .5f));
		}
	}

	public IEnumerator getData()
	{
		FirebaseRealtimeManager.Instance.readStoreImgs(LoginSceneManager.user.storeName);
		yield return WaitServer.Instance.waitServer();
	}

	/// <summary>
	/// Returns Image Component.
	/// </summary>
	/// <returns>Image component</returns>
	public static Image GetUIImage()
	{
		Debug.Log("This is Upload Image");
		return GameObject.Find("Img_UploadImg").GetComponent<Image>();

		//Texture2D newPhoto = new Texture2D(1, 1);
		//byte[] imgData = new byte[fileContents.Length];
		//newPhoto.LoadImage(imgData);
		//newPhoto.Apply();
		//Sprite sprite = Sprite.Create(newPhoto, new Rect(0, 0, newPhoto.width, newPhoto.height), new Vector2(.5f, .5f));
		//img.sprite = sprite;,

		//StartCoroutine(GetTexture(img, uri));
	}

	public void readStoreImgsCorutine()
	{
		StartCoroutine(readStoreImgs());
	}

	IEnumerator readStoreImgs()
	{
		//storeName = "계절밥상"; // Test를 위해서 Firebase에 맞게함. 실제로는 로그인 유저에 맞는 public storeName를 사용하면 됨.
		FirebaseRealtimeManager.Instance.readStoreImgs(storeName); //DB에 저장된 이미지들의 정보를 가져옴
		yield return WaitServer.Instance.waitServer();
		ListStoreImgs = FirebaseRealtimeManager.Instance.ListStoreImgs;
		print($"데이터 가져온 갯수: {ListStoreImgs.Count}");
		ListStoreImgs.Sort(StoreImg.sortOrdercmp);
		foreach (var i in ListStoreImgs)
		{
			i.printAllValues();
			GameObject newItem = Instantiate(item, itemParent);
			newItem.GetComponentInChildren<TextMeshProUGUI>().text = i.imgPath;
			Debug.Log($"newItem {newItem.GetComponentInChildren<TextMeshProUGUI>().text}");
			Debug.Log("--------------------");
		}
		BtnInvoke();
	}

	void BtnInvoke()
	{
		if (itemParent.childCount > 0)
		{
			Debug.Log($"itemInvoke {itemParent.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text}");
			itemParent.GetChild(0).transform.Find("TMP_Item").GetComponent<Button>().onClick.Invoke();
		}
	}

	public void init()
	{
		// 1. DAO 클래스에 담기 + 클래스 sortOrder 순으로 정렬
		readStoreImgsCorutine();
		//ListStoreImgs.Sort() => order로 정렬되도록 함수짜기!
		// 2. 클래스의 길이가 0이 아니라면, 첫번째 sortOrder에 있는 이미지 뿌려주기 void Load()로
	}
}
