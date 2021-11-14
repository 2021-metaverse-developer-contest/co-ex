using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
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
        UploadSceneManager.ListStoreImgs.Add(data);
        newItem.transform.Find("TMP_Item").GetComponent<Button>().onClick.Invoke();
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

    void LoadCoroutine(string imgPath)
    {
        StartCoroutine(Load(imgPath));
    }

    IEnumerator Load(string imgPath)
    {
        StoreImg data = new StoreImg(imgPath);
        FirebaseStorageManager.Instance.LoadFile(data);
        yield return WaitServer.Instance.waitServer();

        Uri uri = FirebaseStorageManager.uri;
        Image img = UploadSceneManager.GetUIImage();
        Debug.Log("In coroutine " + uri.OriginalString);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
        StartCoroutine(WaitServer.Instance.waitServer());
        yield return www.SendWebRequest();
        WaitServer.Instance.isDone = true;
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

        string imgPath = "";
        if (clickObj == null || clickObj.name != "TMP_Item")
        {
            Debug.Log("Click Object is null");
            imgPath = recentItem.GetComponent<TextMeshProUGUI>().text;
        }
        else
        {
            Debug.Log($"Click Object is not null {clickObj.name}");
            imgPath = clickObj.GetComponent<TextMeshProUGUI>().text;
        }
        //string imgPath = clickObj.GetComponent<TextMeshProUGUI>() == null ? recentItem.GetComponent<TextMeshProUGUI>().text : clickObj.GetComponent<TextMeshProUGUI>().text;
        Debug.Log("Click ImagePath " + imgPath);
        LoadCoroutine(imgPath);
    }

    public void SaveBtnOnClick()
    {
        CreateStoreImgCoroutine();
    }

    Transform GetItemsParent()
    {
        return GameObject.Find("ContentUpload").transform;
    }

    void CreateStoreImgCoroutine()
    {
        StartCoroutine(CreateStoreImg());
    }

    IEnumerator CreateStoreImg()
    {
        List<string> imgPathList = new List<string>();
        Transform itemsParent = GetItemsParent();
        string imgPath = "";

        Debug.Log($"childCount {itemsParent.childCount}");
        Debug.Log($"ListCount {UploadSceneManager.ListStoreImgs.ToArray().Length}");

        FirebaseRealtimeManager.Instance.deleteStoreImgs(LoginSceneManager.user.storeName);
        yield return WaitServer.Instance.waitServer();

        for (int i = 0; i < itemsParent.childCount; i++)
        {
            imgPath = itemsParent.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text;
            imgPathList.Add(imgPath);
            foreach (var storeImg in UploadSceneManager.ListStoreImgs)
            {
                if (storeImg.imgPath == imgPath)
                {
                    Debug.Log($"create {i}번째 아이템 {imgPath}");
                    Debug.Log($"storeImg {storeImg.imgPath}");
                    storeImg.sortOrder = i;
                    FirebaseRealtimeManager.Instance.createStoreImg(storeImg);
                    yield return WaitServer.Instance.waitServer();
                    UploadSceneManager.ListStoreImgs.Remove(storeImg);
                    break;
                }
            }
        }
        foreach (var storeImg in UploadSceneManager.ListStoreImgs)
        {
            Debug.Log("Delete Storage?");
            FirebaseStorageManager.Instance.deleteFile(storeImg);
            yield return WaitServer.Instance.waitServer();
        }
#if UNITY_EDITOR
        Debug.Log("저장되었습니다.");
#elif UNITY_ANDROID
		Toast.ShowToastMessage("저장되었습니다.", 3000);
#endif
        if (UploadSceneManager.isBeforeMenu)
        {
            Stack.Instance.Clear();
            //Stack.Instance.Push(new SceneInfo(SceneManager.GetSceneByName("AllCategoryScene")));
        }
        SceneManager.LoadSceneAsync("StoreScene");
        StoreSceneManager.storeName = LoginSceneManager.user.storeName;
    }

    public void CloseUploadBtnOnClick()
    {
        GameObject.Find("Canvas_UploadPop").transform.Find("Panel_PopCloseUpload").gameObject.SetActive(true);
    }

    public void CancelCloseBtnOnClick()
    {
        GameObject.Find("Panel_PopCloseUpload").SetActive(false);
    }

    public void OKCloseBtnOnClick()
    {
        SceneManager.UnloadSceneAsync("UploadScene");
    }
}
