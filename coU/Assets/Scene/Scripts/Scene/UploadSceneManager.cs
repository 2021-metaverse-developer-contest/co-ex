using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class UploadSceneManager : MonoBehaviour
{
    public List<StoreImg> ListStoreImgs;
    public string srcFullPath;
    public string destFullPath;
    public string storeName;
    public string extension;
    public int sortOrder;

    private void Awake()
    {
        ListStoreImgs = new List<StoreImg>();
        this.storeName = LoginSceneManager.user?.storeName;
        this.sortOrder = 0;
    }
    
	private void Start()
	{
        Screen.orientation = ScreenOrientation.Portrait;
        GameObject.Find("TMP_StoreName").GetComponent<TextMeshProUGUI>().text = storeName;
        //LoadCoroutine();
        readStoreImgsCorutine();
    }

	private void Update()
    {
        //this.storeName = LoginSceneManager.user?.storeName;
        //this.sortOrder = 0;
        //FirebaseRealtimeManager.Instance.readValue<StoreImg>(LoginSceneManager.user.id);
    }

    public void getDataCorutine()
    {
        StartCoroutine(getData());
    }

    public void uploadCorutine()
    {
        srcFullPath = "/Users/yunslee/srctest.png";
        extension = srcFullPath.Substring(srcFullPath.LastIndexOf(".") + 1);
        StartCoroutine(Upload());
    }

    public void downloadCorutine()
    {
        destFullPath = "/Users/yunslee/desttest.png";
        StartCoroutine(Download());
    }

    public void LoadCoroutine()
    {
        StartCoroutine(Load());
    }

    public void deleteCorutine()
    {
        StartCoroutine(Delete());
    }

    public IEnumerator Upload()
    {
        StoreImg data = new StoreImg(storeName, extension, sortOrder);
        FirebaseStorageManager.Instance.uploadFile(data, srcFullPath);
        yield return WaitServer.Instance.waitServer();
    }

    public IEnumerator Delete()
    {
        StoreImg data = new StoreImg(storeName, extension, sortOrder);
        FirebaseStorageManager.Instance.deleteFile(data);
        yield return WaitServer.Instance.waitServer();
    }

    public IEnumerator Download()
    {
        StoreImg data = new StoreImg(storeName, extension, sortOrder);
        FirebaseStorageManager.Instance.downloadFile(data, destFullPath);
        yield return WaitServer.Instance.waitServer();
    }

    public IEnumerator Load()
    {
        //StoreImg data = new StoreImg(storeName, extension, sortOrder);
        FirebaseStorageManager.Instance.LoadFile(null);
        yield return WaitServer.Instance.waitServer();
        UpdateTexture(FirebaseStorageManager.fileContent);
    }

    public IEnumerator getData()
    {
        FirebaseRealtimeManager.Instance.readValue<StoreImg>(LoginSceneManager.user.id);
        yield return WaitServer.Instance.waitServer();
    }

    void UpdateTexture(byte[] fileContents)
    {
        Debug.Log("This is Upload Image");
        Texture2D newPhoto = new Texture2D(1, 1);
        newPhoto.LoadImage(fileContents);
        newPhoto.Apply();
        Sprite sprite = Sprite.Create(newPhoto, new Rect(0, 0, newPhoto.width, newPhoto.height), new Vector2(.5f, .5f));
        Image img = GameObject.Find("Img_UploadLogo").GetComponent<Image>();
        img.sprite = sprite;
    }

    public void readStoreImgsCorutine()
    {
        StartCoroutine(readStoreImgs());
    }
    IEnumerator readStoreImgs()
    {
        storeName = "계절밥상"; // Test를 위해서 Firebase에 맞게함. 실제로는 로그인 유저에 맞는 public storeName를 사용하면 됨.
        FirebaseRealtimeManager.Instance.readStoreImgs(storeName);
        yield return WaitServer.Instance.waitServer();
        ListStoreImgs = FirebaseRealtimeManager.Instance.ListStoreImgs;
        print($"데이터 가져온 갯수: {ListStoreImgs.Count}");
        foreach (var i in ListStoreImgs)
        {
            i.printAllValues();
            Debug.Log("--------------------");
        }
    }

}
