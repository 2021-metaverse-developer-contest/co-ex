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
	public string destFullPath;
	public string storeName;
	public string imgType;
	public int sortOrder;

	private void Awake()
	{
		if (ListStoreImgs == null)
			ListStoreImgs = new List<StoreImg>();
		ListStoreImgs.Clear();
		this.storeName = LoginSceneManager.user?.storeName;
		Debug.Log("UploadScene StoreName " + storeName);
		this.sortOrder = 0;
	}

	private void Start()
	{
		Screen.orientation = ScreenOrientation.Portrait;
		GameObject.Find("TMP_StoreName").GetComponent<TextMeshProUGUI>().text = storeName;
		//LoadCoroutine();
		itemParent = GameObject.Find("ContentUpload").transform;
		init();
	}

	private void Update()
	{
		//this.storeName = LoginSceneManager.user?.storeName;
		//this.sortOrder = 0;
		//FirebaseRealtimeManager.Instance.readValue<StoreImg>(LoginSceneManager.user.id);
	}

	public void getDataCoroutine()
	{
		StartCoroutine(getData());
	}

	public void downloadCoroutine()
	{
		destFullPath = "/Users/yunslee/desttest.png";
		StartCoroutine(Download());
	}

	public void LoadCoroutine(string imgPath)
	{
		StartCoroutine(Load(imgPath));
	}

	public void deleteCoroutine()
	{
		StartCoroutine(Delete());
	}

	public IEnumerator Delete()
	{
		StoreImg data = new StoreImg(storeName, imgType, sortOrder);
		FirebaseStorageManager.Instance.deleteFile(data);
		yield return WaitServer.Instance.waitServer();
	}

	public IEnumerator Download()
	{
		StoreImg data = new StoreImg(storeName, imgType, sortOrder);
		FirebaseStorageManager.Instance.downloadFile(data, destFullPath);
		yield return WaitServer.Instance.waitServer();
	}

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
	/// <returns></returns>
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

	//IEnumerator GetTexture(Image img, Uri uri)
	//{
	//	UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
	//	yield return www.SendWebRequest();
	//	if (www.isNetworkError || www.isHttpError)
	//		Debug.Log(www.error);
	//	else
	//	{
	//		Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
	//		img.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(.5f, .5f));
	//	}
	//}

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
			Debug.Log("--------------------");
		}
		BtnInvoke();
	}

	void BtnInvoke()
	{
		if (itemParent.childCount > 0)
			itemParent.GetChild(0).transform.Find("TMP_Item").GetComponent<Button>().onClick.Invoke();
	}

	public void init()
	{
		// 1. DAO 클래스에 담기 + 클래스 sortOrder 순으로 정렬
		readStoreImgsCorutine();
		//ListStoreImgs.Sort() => order로 정렬되도록 함수짜기!
		// 2. 클래스의 길이가 0이 아니라면, 첫번째 sortOrder에 있는 이미지 뿌려주기 void Load()로
	}
	public void attachImage()
	{
		// 0. StoreImgs객체를 만드는게 처음 할일
		//      StoreImgs(storeName, imgType, sortOrder)에서 imgType만 구해서 넣어준다.(storeName과 sortOrder는 이미 정해져있음)
		// 1. image List UI 하나 맨 아래 추가
		// 2. Realtime DB에 저장하기
		// 3. storage에 업로드하ㄱ
	}

	public void detachImage()
	{
		// 1. DeleteItemBtnOnClick을 누르면, 하이어라키에서 해당 리스트가 지워진다.
		// 2. Text에 적혀있었던 imgName과 List.Remove()를 이용하여 이미지리스트에서 해당 부분 삭제
		// 3. DB에서 해당 부분만 삭제하기 => "1_계절밥상" Key값을 객체 변수로부터 알 수 가 없음.
		// 따라서 현재로썬 일딴은 다 지우고, 리스트로 저장되어있는 storeImgs로 갱신하는 것으로 하기
		// 4. storage에서도 imgPath를 이용하여 삭제
	}

	// 다른 함수와 달리 Storage를 변경하지 않아도 된다.
	public void changeOrderImage()
	{
		// 1. image List를 이동시키는 것을 감지하면,
		// 2. List의 순서를 변경하고,
		// 3. DB에 저장시킨다.
	}
}
