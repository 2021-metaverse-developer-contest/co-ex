using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class FirebaseRealtimeManager
{
    private static FirebaseRealtimeManager _realtimeDB = null;
    private static DatabaseReference dbReferecne = null;

    // Realtime DB의 값을 여기에 가져다 줌, FirebaseStorageManager에서는 void 꼴
    public User user = null;
    public StoreImg storeImg = null;
    public List<StoreImg> ListStoreImgs = new List<StoreImg>();

    private FirebaseRealtimeManager()
    {
        Debug.Log("Constructor of FirebaseRealtimeManager");
        dbReferecne = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public static FirebaseRealtimeManager Instance
    {
        get
        {
            if (_realtimeDB == null)
                _realtimeDB = new FirebaseRealtimeManager();
            return _realtimeDB;
        }
    }

    // 구현 안함
    //Action<DatabaseReference> updateValue = (reference) =>
    //{
    //};

    public void printAllValue<T>()
    {
        string table = typeof(T).Name;
        //dbReferecne.Child(table).GetValueAsync().ContinueWithOnMainThread(task =>
        dbReferecne.Child(table).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log(task.Exception);
                WaitServer.Instance.isDone = true;
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log($"ChildrenCount: {snapshot.ChildrenCount}");
                foreach (DataSnapshot snap in snapshot.Children)
                {
                    T data = JsonUtility.FromJson<T>(snapshot.GetRawJsonValue());
                    Debug.Log(data);
                }
                WaitServer.Instance.isDone = true;
            }
        });
    }

    public void readValue<T>(string id)
    {
        string table = typeof(T).Name;
        //dbReferecne.Child(table).Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        dbReferecne.Child(table).Child(id).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log(task.Exception);
                WaitServer.Instance.isDone = true;
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.ChildrenCount);
                T data = JsonUtility.FromJson<T>(snapshot.GetRawJsonValue());
                // "as" data의 형식이 맞지 않는다면, null 값이 들어감
                // 값을 찾지 못해도 null 값이 들어감!
                _realtimeDB.user = data as User;
                Debug.Log("storeName" + _realtimeDB.user.storeName);
                _realtimeDB.storeImg = data as StoreImg;
                WaitServer.Instance.isDone = true;
            }
        });
    }

    public void deleteAllValue<T>()
    {
        string table = typeof(T).Name;
        //dbReferecne.Child(table).GetValueAsync().ContinueWithOnMainThread(task =>
        dbReferecne.Child(table).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log(task.Exception);
                WaitServer.Instance.isDone = true;
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.ChildrenCount);
                foreach (DataSnapshot snap in snapshot.Children)
                {
                    snap.Reference.RemoveValueAsync();
                }
                WaitServer.Instance.isDone = true;
            }
        });
    }


    public void deleteValue<T>(string id)
    {
        string table = typeof(T).Name;
        dbReferecne.Child(table).Child(id).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log(task.Exception);
                WaitServer.Instance.isDone = true;
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                snapshot.Reference.RemoveValueAsync();
                WaitServer.Instance.isDone = true;
            }
        });
    }

    public void createValue<T>(string id, T data)
    {
        string table = typeof(T).Name;
        string json = JsonUtility.ToJson(data);
        dbReferecne.Child(table).Child(id).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log(task.Exception);
                WaitServer.Instance.isDone = true;
            }
            else
            {
                WaitServer.Instance.isDone = true;
            }
        });
    }

    public void readStoreImgs(string storeName)
    {
        string table = typeof(StoreImg).Name;
        //dbReferecne.Child(table).Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        dbReferecne.Child(table).Child(storeName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log(task.Exception);
                WaitServer.Instance.isDone = true;
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Key + "의 파일 갯수:" + snapshot.ChildrenCount); // MiniStoreImgs
                foreach (DataSnapshot snap in snapshot.Children)
                {
                    string imgPath;
                    long sortOrder;
					Debug.Log(snap.Key); // 1_계절밥상
					IDictionary dicts = (IDictionary)snap.Value;
                    imgPath = dicts[nameof(imgPath)] as string;
                    sortOrder = (long)dicts[nameof(sortOrder)];
					StoreImg temp = new StoreImg(imgPath, sortOrder);
                    ListStoreImgs.Add(temp);

					//string storeName;
					//string imgType;
					//long sortOrder;
					//Debug.Log(snap.Key); // 1_계절밥상
					//IDictionary dicts = (IDictionary)snap.Value;
					//// Debug.Log((string)dicts[nameof(id)] + (string)dicts[nameof(storeName)] + ((long)dicts[nameof(sortOrder)]).ToString());
					//storeName = dicts[nameof(storeName)] as string;
					//imgType = dicts[nameof(imgType)] as string;
					//sortOrder = (long)dicts[nameof(sortOrder)];
					//StoreImg temp = new StoreImg(storeName, imgType, sortOrder);
					//ListStoreImgs.Add(temp);
				}
				WaitServer.Instance.isDone = true;
            }
        });
    }
}
