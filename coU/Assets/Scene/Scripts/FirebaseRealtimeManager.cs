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

    public void printAllValue<T>(WaitServer wait)
    {
        string table = typeof(T).Name;
        //dbReferecne.Child(table).GetValueAsync().ContinueWithOnMainThread(task =>
        dbReferecne.Child(table).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log(task.Exception);
                wait.isDone = true;
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
                wait.isDone = true;
            }
        });
    }

    public void readUser(string id, WaitServer wait)
    {
        //dbReferecne.Child(table).Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        dbReferecne.Child("User").Child(id).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log(task.Exception);
                wait.isDone = true;
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.ChildrenCount);
                User data = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
                // "as" data의 형식이 맞지 않는다면, null 값이 들어감
                // 값을 찾지 못해도 null 값이 들어감!
                if (data == null)
                {
                    _realtimeDB.user = null;
                }
                else
                {
                    _realtimeDB.user = data as User;
                    Debug.Log("storeName" + _realtimeDB.user.storeName);
                }
				wait.isDone = true;
            }
        });
    }

    public void deleteAllValue<T>(WaitServer wait)
    {
        string table = typeof(T).Name;
        //dbReferecne.Child(table).GetValueAsync().ContinueWithOnMainThread(task =>
        dbReferecne.Child(table).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log(task.Exception);
                wait.isDone = true;
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.ChildrenCount);
                foreach (DataSnapshot snap in snapshot.Children)
                {
                    snap.Reference.RemoveValueAsync();
                }
                wait.isDone = true;
            }
        });
    }


    public void deleteValue<T>(string id, WaitServer wait)
    {
        string table = typeof(T).Name;
        dbReferecne.Child(table).Child(id).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log(task.Exception);
                wait.isDone = true;
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                snapshot.Reference.RemoveValueAsync();
                wait.isDone = true;
            }
        });
    }

    public void deleteStoreImgs(string storeName, WaitServer wait)
    {
        dbReferecne.Child("StoreImg").Child(storeName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
                Debug.Log("Error Delete StoreImgs: " + task.Exception);
            else
			{
                DataSnapshot snapshot = task.Result;
                snapshot.Reference.RemoveValueAsync();
			}
            wait.isDone = true;
        });
    }

    public void createUser(string id, User newUser, WaitServer wait)
    {
        string json = JsonUtility.ToJson(newUser);
        dbReferecne.Child("User").Child(id).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log(task.Exception);
                wait.isDone = true;
            }
            else
            {
                wait.isDone = true;
            }
        });
    }

    public void createStoreImg(StoreImg newImg, WaitServer wait)
    {
        string json = JsonUtility.ToJson(newImg);
        string key = newImg.storeName;
        long idx = newImg.sortOrder;
        Debug.Log($"In CreateStoreImg key:{key}, idx:{idx}");
        Debug.Log($"Json {json}");
        dbReferecne.Child("StoreImg").Child(key).Child($"{idx}_{key}").SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Error in createStoreImg: " + task.Exception);
                wait.isDone = true;
            }
            else
            {
                Debug.Log("success");
                wait.isDone = true;
            }
            Debug.Log("In CreateSotreImg is done");
        });
    }

    public void readStoreImgs(string storeName, WaitServer wait)
    {
        ListStoreImgs.Clear();
        string table = typeof(StoreImg).Name;
        //dbReferecne.Child(table).Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        dbReferecne.Child(table).Child(storeName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log(task.Exception);
                wait.isDone = true;
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
				wait.isDone = true;
            }
        });
    }
}
