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
using UnityEngine.EventSystems;

public class UploadSceneManager : MonoBehaviour
{
	[SerializeField]
	private GameObject item;
	Transform itemParent;
	public static bool isBeforeMenu = false;

	public static List<StoreImg> ListStoreImgs;
	public string storeName;

	private void Awake()
	{
		if (ListStoreImgs == null)
			ListStoreImgs = new List<StoreImg>();
		ListStoreImgs.Clear();
		this.storeName = DontDestroyManager.LoginScene.user?.storeName;
		Debug.Log("UploadScene StoreName " + storeName);
	}

	private void Start()
	{
		Screen.orientation = ScreenOrientation.Portrait;
		GameObject.Find("Panel_UploadCenter/TMP_StoreName").GetComponent<TextMeshProUGUI>().text = storeName;
		itemParent = GameObject.Find("ContentUpload").transform;
		ListStoreImgs.Clear();
		init();
	}

	private void Update()
	{
	}

	/// <summary>
	/// Returns Image Component.
	/// <returns>Image component</returns>
	/// </summary>
	public static Image GetUIImage()
	{
		Debug.Log("This is Upload Image");
		return GameObject.Find("Img_UploadImg").GetComponent<Image>();
	}

	public void readStoreImgsCorutine()
	{
		StartCoroutine(readStoreImgs());
	}

	IEnumerator readStoreImgs()
	{
		//storeName = "계절밥상"; // Test를 위해서 Firebase에 맞게함. 실제로는 로그인 유저에 맞는 public storeName를 사용하면 됨.
		WaitServer wait = new WaitServer();
		FirebaseRealtimeManager firebaseRealtime = new FirebaseRealtimeManager();
		firebaseRealtime.readStoreImgs(storeName, wait); //DB에 저장된 이미지들의 정보를 가져옴
		yield return wait.waitServer();
		ListStoreImgs = firebaseRealtime.ListStoreImgs;
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
			EventSystem.current.SetSelectedGameObject(itemParent.GetChild(0).transform.Find("TMP_Item").gameObject);
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
