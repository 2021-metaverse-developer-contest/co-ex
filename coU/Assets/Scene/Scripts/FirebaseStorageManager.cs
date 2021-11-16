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
    private Firebase.Storage.FirebaseStorage firebaseStorage;
    private string firebasestorageURL;
    public Uri uri;
    public FirebaseStorageManager()
    {
        firebaseStorage = Firebase.Storage.FirebaseStorage.DefaultInstance;
        firebasestorageURL = "gs://co-ex1.appspot.com";
    }

    public void uploadFile(StoreImg storageData, string srcFullPath, WaitServer wait)
    {
        StorageReference uploadPath = firebaseStorage.GetReferenceFromUrl(storageData.getfullPath(firebasestorageURL));
        uploadPath.PutFileAsync(srcFullPath).ContinueWith((task) =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
                wait.isDone = true;
            }
            else
            {
                // Metadata contains file metadata such as size, content-type, and download URL.
                Firebase.Storage.StorageMetadata metadata = task.Result;
                //string download_url = metadata.DownloadUrl.ToString(); Deprecated
                Debug.Log(metadata.SizeBytes.ToString());
                wait.isDone = true;
            }
        });
    }

    public void downloadFile(StoreImg storageData, string destFullPath, WaitServer wait)
    {
        StorageReference uploadPath = firebaseStorage.GetReferenceFromUrl(storageData.getfullPath(firebasestorageURL));
        uploadPath.GetFileAsync(destFullPath).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
                wait.isDone = true;
            }
            else
            {
                // Metadata contains file metadata such as size, content-type, and download URL.
                //Firebase.Storage.StorageMetadata metadata = task.Result;
                //string download_url = metadata.DownloadUrl.ToString(); Deprecated
                //Debug.Log(metadata.SizeBytes.ToString());
                wait.isDone = true;
            }
        });
    }

    public void deleteFile(StoreImg storageData, WaitServer wait)
    {
        StorageReference deletePath = firebaseStorage.GetReferenceFromUrl(storageData.getfullPath(firebasestorageURL));
        deletePath.DeleteAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
                wait.isDone = true;
            }
            else
            {
                // Metadata contains file metadata such as size, content-type, and download URL.
                //Firebase.Storage.StorageMetadata metadata = task.Result;
                //string download_url = metadata.DownloadUrl.ToString(); Deprecated
                //Debug.Log(metadata.SizeBytes.ToString());
                wait.isDone = true;
            }
        });
    }

    //public static Stream fileContents = null;

    public void LoadFile(StoreImg storageData, WaitServer wait)
    {
        Debug.Log("LoadFile 102 " + storageData.getfullPath(firebasestorageURL));
        StorageReference loadPath = firebaseStorage.GetReferenceFromUrl(storageData.getfullPath(firebasestorageURL));
        //const long maxAllowedSize = long.MaxValue;
        //loadPath.GetStreamAsync().ContinueWith(task =>
        //{
        //    if (task.IsFaulted || task.IsCanceled)
        //    {
        //        Debug.Log("Error " + task.Exception.ToString());
        //        wait.isDone = true;
        //    }
        //    else
        //    {
        //        fileContents = task.Result;
        //        Debug.Log("Finished downloading");
        //        wait.isDone = true;
        //    }
        //});

        loadPath.GetDownloadUrlAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
                Debug.Log("Error" + task.Exception.ToString());
            else
            {
                uri = task.Result;
                Debug.Log("StorageManager uri string " + uri.OriginalString);
                Debug.Log("Finished downloading");
            }
            wait.isDone = true;
        });
    }
}
