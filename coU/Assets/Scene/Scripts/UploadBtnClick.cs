using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadBtnClick : MonoBehaviour
{
    public void GalleryBtnOnClick()
    {
        print("Is in?");
        NativeGallery.Permission permission = NativeGallery.GetMixedMediasFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
        }, NativeGallery.MediaType.Image | NativeGallery.MediaType.Video, "Select images or videos");
    }
}
