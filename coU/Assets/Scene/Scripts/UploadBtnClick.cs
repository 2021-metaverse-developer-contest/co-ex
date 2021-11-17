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
    List<StoreImg> listStoreImgs = new List<StoreImg>();

    void UploadCoroutine(string srcFullPath)
    {
        //srcFullPath = "/Users/yunslee/srctest.png";
        StartCoroutine(Upload(srcFullPath));
    }

    IEnumerator Upload(string srcFullPath)
    {
        string imgType = srcFullPath.Substring(srcFullPath.LastIndexOf(".") + 1);
        StoreImg data = new StoreImg(LoginSceneManager.user?.storeName, imgType, 0);
		WaitServer wait = new WaitServer();
        FirebaseStorageManager firebaseStorage = new FirebaseStorageManager();
        firebaseStorage.uploadFile(data, srcFullPath, wait);
        yield return wait.waitServer();
        GameObject newItem = Instantiate(item, GameObject.Find("ContentUpload").transform);
        newItem.GetComponentInChildren<TextMeshProUGUI>().text = data.imgPath;
        UploadSceneManager.ListStoreImgs.Add(data);

        EventSystem.current.SetSelectedGameObject(newItem.transform.Find("TMP_Item").gameObject);
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
            UploadCoroutine(newPath);
#elif UNITY_ANDROID
            UploadCoroutine("file://" + newPath);
#endif
            //GameObject newItem = Instantiate(item, GameObject.Find("Content").transform);
            //newItem.GetComponentInChildren<TextMeshProUGUI>().text = newPath;
        }, NativeGallery.MediaType.Image | NativeGallery.MediaType.Video, "Select images or videos");

        Debug.Log("Permission is " + permission);
    }

    public void DeleteItemBtnOnClick()
    {
        GameObject deleteObj = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        Color deleteClr = deleteObj.GetComponent<Image>().color;

        //Destroy(deleteObj);
        DestroyImmediate(deleteObj);
        Transform content = GameObject.Find("ContentUpload").transform;
        Debug.Log($"Content child count {content.childCount}");
        if (content.childCount > 0)
        {
            if (deleteClr == new Color32(198, 215, 255, 76))
            {
                Debug.Log("0 is not null");
                Debug.Log($"0 is {content.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text}");
                EventSystem.current.SetSelectedGameObject(content.GetChild(0).Find("TMP_Item").gameObject);
                content.GetChild(0).Find("TMP_Item").GetComponent<Button>().onClick.Invoke();
            }
        }
        else
        {
            Image img = UploadSceneManager.GetUIImage();
            img.sprite = null;
        }
    }

    void LoadCoroutine(string imgPath)
    {
        StartCoroutine(Load(imgPath));
    }

    IEnumerator Load(string imgPath)
    {
        StoreImg data = new StoreImg(imgPath);
		WaitServer wait = new WaitServer();
        FirebaseStorageManager firebaseStorage = new FirebaseStorageManager();
        firebaseStorage.LoadFile(data, wait);
        yield return wait.waitServer();

        Uri uri = firebaseStorage.uri;
        Image img = UploadSceneManager.GetUIImage();
        Debug.Log("In coroutine " + uri.OriginalString);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
        WaitServer wait2 = new WaitServer();
        StartCoroutine(wait2.waitServer());
        yield return www.SendWebRequest();
        wait2.isDone = true;
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
        int childCount = contentList.childCount;
        GameObject recentItem = contentList.GetChild(childCount - 1).Find("TMP_Item").gameObject;
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;

        if (clickObj == null || clickObj.name != "TMP_Item")
            clickObj = recentItem;

        // 아이템이 클릭됐을 때 색상 변화가 있어야함.
        // 다른 곳을 클릭하면 색상이 그대로 유지되어야하되 다른 아이템을s 클릭하면 색상이 화이트로 변경되어야함.
        // 한마디로 아이템들이 하나의 토글 그룹이라고 생각하면 됨.
        for (int i = 0; i < childCount; i++)
        {
            contentList.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            if (contentList.GetChild(i) == clickObj.transform.parent)
                contentList.GetChild(i).GetComponent<Image>().color = new Color32(198, 215, 255, 76);
        }

        LoadCoroutine(clickObj.GetComponent<TextMeshProUGUI>().text);
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

		WaitServer wait = new WaitServer();
        FirebaseRealtimeManager firebaseRealtime = new FirebaseRealtimeManager();
        firebaseRealtime.deleteStoreImgs(LoginSceneManager.user.storeName, wait);
        yield return wait.waitServer();

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
		            WaitServer wait2 = new WaitServer();
                    firebaseRealtime.createStoreImg(storeImg, wait2);
                    yield return wait2.waitServer();
                    UploadSceneManager.ListStoreImgs.Remove(storeImg);
                    break;
                }
            }
        }
        foreach (var storeImg in UploadSceneManager.ListStoreImgs)
        {
            Debug.Log("Delete Storage?");
		    WaitServer wait3 = new WaitServer();
            new FirebaseStorageManager().deleteFile(storeImg, wait3);
            yield return wait3.waitServer();
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
