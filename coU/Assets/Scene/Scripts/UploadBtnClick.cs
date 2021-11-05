using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UploadBtnClick : MonoBehaviour
{
    [SerializeField]
    private GameObject item; //TMP_Item UI에 컨텐츠 이름 넣긴

    public void GalleryBtnOnClick()
    {
        string newPath = "";
        print("Is in?");
        NativeGallery.Permission permission = NativeGallery.GetMixedMediaFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            newPath = path;
            GameObject newItem = Instantiate(item, GameObject.Find("Content").transform);
            newItem.GetComponentInChildren<TextMeshProUGUI>().text = newPath;
        }, NativeGallery.MediaType.Image | NativeGallery.MediaType.Video, "Select images or videos");


        Debug.Log("Permission is " + permission);
    }

    public void DeleteItemBtnOnClick()
    {
        GameObject deleteObj = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;

        Destroy(deleteObj);
    }
}
