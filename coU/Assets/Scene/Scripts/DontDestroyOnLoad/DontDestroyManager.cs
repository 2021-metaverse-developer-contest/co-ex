using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyManager : MonoBehaviour
{
    static public DontDestroyData.LoginSceneData LoginScene;
    static public DontDestroyData.UploadSceneData UploadScene;
    static public DontDestroyData.StoreListSceneData StoreList;
    static public DontDestroyData.SearchSceneData SearchScene;
    static public DontDestroyData.SelectSceneData SelectStoreScene;


    void Awake()
    {
        if (FindObjectsOfType<DontDestroyManager>().Length > 0)
		{
            DontDestroyData ee = new DontDestroyData();
			DontDestroyOnLoad(this);
		}
		else
			Destroy(this);
	}
    void Start()
    {
        LoginScene = new DontDestroyData.LoginSceneData();
        UploadScene = new DontDestroyData.UploadSceneData();
        StoreList = new DontDestroyData.StoreListSceneData();
        SearchScene = new DontDestroyData.SearchSceneData();
        SelectStoreScene = new DontDestroyData.SelectSceneData();
    }
}
