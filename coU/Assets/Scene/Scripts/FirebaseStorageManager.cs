using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Firebase;
using Firebase.Auth;
using Firebase.Storage;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseStorageManager
{
    private static FirebaseStorageManager _firebaseStorageManager;
    public static FirebaseStorageManager Instance
    {
        get
        {
            if (_firebaseStorageManager == null)
                _firebaseStorageManager = new FirebaseStorageManager();
            return _firebaseStorageManager;
        }
    }

    private Firebase.Storage.FirebaseStorage firebaseStorage;
    private string firebasestorageURL;
    private FirebaseStorageManager()
    {
        firebaseStorage = Firebase.Storage.FirebaseStorage.DefaultInstance;
        firebasestorageURL = "gs://co-ex1.appspot.com";
    }

    public void uploadFile(StoreImg storageData, string srcFullPath)
    {
        StorageReference uploadPath = firebaseStorage.GetReferenceFromUrl(storageData.getfullPath(firebasestorageURL));
        uploadPath.PutFileAsync(srcFullPath).ContinueWith((task) =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                // Metadata contains file metadata such as size, content-type, and download URL.
                Firebase.Storage.StorageMetadata metadata = task.Result;
                //string download_url = metadata.DownloadUrl.ToString(); Deprecated
                Debug.Log(metadata.SizeBytes.ToString());
                LoginSceneManager.isDone = true;
            }
        });
    }

    public void downloadFile(StoreImg storageData, string destFullPath)
    {
        StorageReference uploadPath = firebaseStorage.GetReferenceFromUrl(storageData.getfullPath(firebasestorageURL));
        uploadPath.GetFileAsync(destFullPath).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                // Metadata contains file metadata such as size, content-type, and download URL.
                //Firebase.Storage.StorageMetadata metadata = task.Result;
                //string download_url = metadata.DownloadUrl.ToString(); Deprecated
                //Debug.Log(metadata.SizeBytes.ToString());
                LoginSceneManager.isDone = true;
            }
        });
    }

    public void deleteFile(StoreImg storageData)
    {
        StorageReference deletePath = firebaseStorage.GetReferenceFromUrl(storageData.getfullPath(firebasestorageURL));
        deletePath.DeleteAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                // Metadata contains file metadata such as size, content-type, and download URL.
                //Firebase.Storage.StorageMetadata metadata = task.Result;
                //string download_url = metadata.DownloadUrl.ToString(); Deprecated
                //Debug.Log(metadata.SizeBytes.ToString());
                LoginSceneManager.isDone = true;
            }
        });
    }
}
