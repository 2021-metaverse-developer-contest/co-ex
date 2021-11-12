using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UploadBtnClick : MonoBehaviour
{
    [SerializeField]
    private GameObject item; //TMP_Item UI에 컨텐츠 이름 넣긴

    public void uploadCoroutine(string srcFullPath)
    {
        //srcFullPath = "/Users/yunslee/srctest.png";
        StartCoroutine(Upload(srcFullPath));
    }

    public IEnumerator Upload(string srcFullPath)
    {
        string imgType = srcFullPath.Substring(srcFullPath.LastIndexOf(".") + 1);
        StoreImg data = new StoreImg(LoginSceneManager.user?.storeName, imgType, 0);
        FirebaseStorageManager.Instance.uploadFile(data, srcFullPath);
        yield return WaitServer.Instance.waitServer();
        GameObject newItem = Instantiate(item, GameObject.Find("ContentUpload").transform);
        newItem.GetComponentInChildren<TextMeshProUGUI>().text = data.imgPath;
    }

    public void GalleryBtnOnClick()
    {
        string newPath = "";
        Debug.Log("Gallery Btn on click");
        NativeGallery.Permission permission = NativeGallery.GetMixedMediaFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            newPath = path;
            uploadCoroutine(newPath);
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
}
