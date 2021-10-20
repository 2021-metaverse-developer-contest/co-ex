using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CopyDB : MonoBehaviour
{
    private static string dbName = "Starfield.db";
    private static int count = 0;
    private static string persistentDBTotalPath;
    private static string streamingAssetsDBTotalPath;

    public string homeSceneName;

    private void Awake()
    {
        // 파일 경로
        persistentDBTotalPath = Path.Combine(Application.persistentDataPath, dbName);
        streamingAssetsDBTotalPath = Path.Combine(Application.streamingAssetsPath, dbName);
        // 파일이 핸드폰/노틔북에 남아있다면 우선 삭제한다.
        DeleteFile();
        // streamingAssetsDBTotalPath에 파일이 없다면, 프로그램 종료에 대해서 작성해야함
        if (File.Exists(streamingAssetsDBTotalPath) == false)
        {
            print("File.Exists(streamingAssetsDBTotalPath) == false");
            #if !UNITY_EDITOR && UNITY_ANDROID
            #elif (!UNITY_EDITOR && UNITY_IOS)
            #elif (UNITY_EDITOR)
            #endif
        }
        // Copy 시작
#if !UNITY_EDITOR && UNITY_ANDROID
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(streamingAssetsDBTotalPath);
        UnityWebRequestAsyncOperation timetime = unityWebRequest.SendWebRequest();
        while (timetime.isDone == false)
        {;}
        //StartCoroutine(waitUntilSendAllBytes(unityWebRequest));
        print($"{unityWebRequest.downloadHandler.data.Length} 파일의 크기");
        File.WriteAllBytes(persistentDBTotalPath, unityWebRequest.downloadHandler.data);
        /* 나중에 성능 개선할 때 생각해보도록 한다.
        UnityWebRequest unityWebRequest;
        do
        {
            count++; print(count);
            unityWebRequest = UnityWebRequest.Get(streamingAssetsDBTotalPath);
            StartCoroutine(waitUntilSendAllBytes(unityWebRequest));
            print($"downloadHandler: {unityWebRequest.downloadHandler.data.Length} 파일의 크기");
            print($"uploadHandler: {unityWebRequest.uploadHandler.data.Length} 파일의 크기");
        }
        while (unityWebRequest.downloadHandler.data.Length != unityWebRequest.uploadHandler.data.Length);
        print($"{unityWebRequest.downloadHandler.data.Length} 파일의 크기");
        File.WriteAllBytes(persistentDBTotalPath, unityWebRequest.downloadHandler.data);
        */
#elif (UNITY_EDITOR) || (!UNITY_EDITOR && UNITY_IOS)
        File.Copy(streamingAssetsDBTotalPath, persistentDBTotalPath);
#endif
    }


    IEnumerator waitUntilSendAllBytes(UnityWebRequest unityWebRequest)
    {
        yield return unityWebRequest.SendWebRequest();
    }

    static void DeleteFile()
    {
        if (File.Exists(persistentDBTotalPath) == false)
        {
            string temp = string.Format("{0} 파일 없음", persistentDBTotalPath);
            print(temp);
        }
        else
        {
            string temp = string.Format("{0} 파일 있음", persistentDBTotalPath);
            print(temp);
            FileInfo fInfo = new FileInfo(persistentDBTotalPath);
            print($"파일의 크기: {fInfo.Length}");
            File.Delete(persistentDBTotalPath);
            print("파일 삭제함");
        }
    }

    void Start()
    {
        print("Start()");
        print(persistentDBTotalPath);
        print($"Load Scene {homeSceneName}");
        SceneManager.LoadScene(homeSceneName);
    }
}
