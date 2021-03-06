using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DontDestroyManager : MonoBehaviour
{
    static public DontDestroyData.LoginSceneData LoginScene;
    static public DontDestroyData.UploadSceneData UploadScene;
    static public DontDestroyData.StoreListSceneData StoreListScene;
    static public DontDestroyData.SearchSceneData SearchScene;
    static public DontDestroyData.SelectSceneData SelectStoreScene;
    static public DontDestroyData.RegisterSceneData RegisterScene;
    static public DontDestroyData.StoreSceneData StoreScene;
    static public DontDestroyData.MaxstSceneData MaxstScene;

    private string currentSceneName;

    void Awake()
    {
        if (FindObjectsOfType<DontDestroyManager>().Length > 0)
		{
            DontDestroyData ee = new DontDestroyData();
			DontDestroyOnLoad(this);
		}
		else
			Destroy(this);
        currentSceneName = "MaxstScene";
        LoginScene = new DontDestroyData.LoginSceneData();
        UploadScene = new DontDestroyData.UploadSceneData();
        StoreListScene = new DontDestroyData.StoreListSceneData();
        SearchScene = new DontDestroyData.SearchSceneData();
        SelectStoreScene = new DontDestroyData.SelectSceneData();
        RegisterScene = new DontDestroyData.RegisterSceneData();
        StoreScene = new DontDestroyData.StoreSceneData();
        MaxstScene = new DontDestroyData.MaxstSceneData();
	}
    void Start()
    {
    }





    static public void newPush(string sceneName_, string storeName_ = "", string categorySub_ = "")
	{
        SceneInfo element = new SceneInfo(beforeSceneStr: sceneName_, storeName: storeName_, categorySub: categorySub_);
        Stack.Instance.Push(element);
    }

    static public SceneInfo newPop()
    {
        return (Stack.Instance.Pop());
    }

    static public string getSceneName(EventSystem cur)
    {
        GameObject clickObj = cur.currentSelectedGameObject;
        return (clickObj.scene.name);
    }
}
