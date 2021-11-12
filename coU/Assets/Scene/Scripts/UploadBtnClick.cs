using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UploadBtnClick : MonoBehaviour
{
    [SerializeField]
    private GameObject item; //TMP_Item UI에 컨텐츠 이름 넣긴

    void uploadCoroutine(string srcFullPath)
    {
        //srcFullPath = "/Users/yunslee/srctest.png";
        StartCoroutine(Upload(srcFullPath));
    }

    IEnumerator Upload(string srcFullPath)
    {
        string imgType = srcFullPath.Substring(srcFullPath.LastIndexOf(".") + 1);
        StoreImg data = new StoreImg(LoginSceneManager.user?.storeName, imgType, 0);
        FirebaseStorageManager.Instance.uploadFile(data, srcFullPath);
        yield return WaitServer.Instance.waitServer();
        GameObject newItem = Instantiate(item, GameObject.Find("ContentUpload").transform);
        newItem.GetComponentInChildren<TextMeshProUGUI>().text = data.imgPath;
        //newItem.transform.Find("TMP_Item").GetComponent<Button>().onClick.Invoke();
    }

    public void GalleryBtnOnClick()
    {
        GameObject contentList = GameObject.Find("ContentUpload");

        if (contentList.transform.childCount > 4)
		{
#if UNITY_EDITOR
            Debug.Log("올릴 수 있는 콘텐츠의 개수는 최대 5개입니다.");
#elif UNITY_ANDROID
            Toast.ShowToastMessage("올릴 수 있는 콘텐츠의 개수는 최대 5개입니다.", 3000);
#endif
            return;
		}
        string newPath = "";
        Debug.Log("Gallery Btn on click");
        NativeGallery.Permission permission = NativeGallery.GetMixedMediaFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            newPath = path;
#if UNITY_EDITOR
            uploadCoroutine(newPath);
#elif UNITY_ANDROID
            uploadCoroutine("file://" + newPath);
#endif
            //GameObject newItem = Instantiate(item, GameObject.Find("Content").transform);
            //newItem.GetComponentInChildren<TextMeshProUGUI>().text = newPath;
        }, NativeGallery.MediaType.Image | NativeGallery.MediaType.Video, "Select images or videos");

        Debug.Log("Permission is " + permission);
    }

    public void DeleteItemBtnOnClick()
    {
        GameObject deleteObj = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;

        Destroy(deleteObj);
    }

    void LoadCoroutine(string imgPath, Uri uri)
    {
        StartCoroutine(Load(imgPath, uri));
    }

    Image UpdateTexture(Uri uri)
    {
        Debug.Log("This is Upload Image");
        //Texture2D newPhoto = new Texture2D(1, 1);
        //byte[] imgData = new byte[fileContents.Length];
        //newPhoto.LoadImage(imgData);
        //newPhoto.Apply();
        //Sprite sprite = Sprite.Create(newPhoto, new Rect(0, 0, newPhoto.width, newPhoto.height), new Vector2(.5f, .5f));
        return GameObject.Find("Img_UploadImg").GetComponent<Image>();
        //StartCoroutine(GetTexture(img, uri));
        //img.sprite = sprite;,
    }

    //IEnumerator GetTexture(Image img, Uri uri)
    //{
    //    Debug.Log("In coroutine " + uri.OriginalString);
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
        
    //    yield return www.SendWebRequest();
    //    if (www.isNetworkError || www.isHttpError)
    //        Debug.Log(www.error);
    //    else
    //    {
    //        Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        Sprite sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(.5f, .5f));
    //        img.sprite = sprite;
    //    }
    //}

    IEnumerator Load(string imgPath, Uri uri)
	{
        StoreImg data = new StoreImg(imgPath);
        FirebaseStorageManager.Instance.LoadFile(data);
        yield return WaitServer.Instance.waitServer();
        Image img = UpdateTexture(uri);
        Debug.Log("In coroutine " + uri.OriginalString);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);

        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
            Debug.Log(www.error);
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(.5f, .5f));
            img.sprite = sprite;
        }
        //StartCoroutine(GetTexture(img, uri));
        //UploadSceneManager.UpdateTexture(FirebaseStorageManager.fileContents);
    }

    public void UploadItemOnClick()
	{
        Transform contentList = GameObject.Find("ContentUpload").transform;
        GameObject recentItem = contentList.GetChild(contentList.childCount - 1).Find("TMP_Item").gameObject;
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;

        string imgPath = clickObj.GetComponent<TextMeshProUGUI>() == null ?  recentItem.GetComponent<TextMeshProUGUI>().text: clickObj.GetComponent<TextMeshProUGUI>().text;
        Debug.Log("Click ImagePath " + imgPath);
        LoadCoroutine(imgPath, FirebaseStorageManager.uri);
	}
}
