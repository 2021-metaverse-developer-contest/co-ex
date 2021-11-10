using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UploadManager : MonoBehaviour
{
    public List<StoreImg> storeImgList;
    public string srcFullPath;
    public string destFullPath;
    public string storeName;
    public string extension;
    public int sortOrder;

    private void Awake()
    {
        storeImgList = new List<StoreImg>();
        this.storeName = LoginSceneManager.user?.storeName;
        this.sortOrder = 0;
    }

	private void Start()
	{
        Screen.orientation = ScreenOrientation.Portrait;
    }

	private void Update()
    {
        this.storeName = LoginSceneManager.user?.storeName;
        this.sortOrder = 0;
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

    public IEnumerator getData()
    {
        FirebaseRealtimeManager.Instance.readValue<StoreImg>(LoginSceneManager.user.id);
        yield return WaitServer.Instance.waitServer();
    }

}
