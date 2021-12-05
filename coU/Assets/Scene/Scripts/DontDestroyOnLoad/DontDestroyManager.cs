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
	}
    void Start()
    {
        LoginScene = new DontDestroyData.LoginSceneData();
        UploadScene = new DontDestroyData.UploadSceneData();
        StoreListScene = new DontDestroyData.StoreListSceneData();
        SearchScene = new DontDestroyData.SearchSceneData();
        SelectStoreScene = new DontDestroyData.SelectSceneData();
        RegisterScene = new DontDestroyData.RegisterSceneData();
        StoreScene = new DontDestroyData.StoreSceneData();
        MaxstScene = new DontDestroyData.MaxstSceneData();
    }


    static public void newPush2(string sceneName_)
    {
        SceneInfo element = null;
        switch (sceneName_)
        {
            case SceneName.AllCategoryScene:
                element = new SceneInfo(beforeSceneStr: sceneName_, categorySub: DontDestroyManager.StoreListScene.categorySub);
                break;
            case SceneName.MaxstScene:
                element = new SceneInfo(beforeSceneStr: sceneName_);
                break;
            case SceneName.SearchScene:
                element = new SceneInfo(beforeSceneStr: sceneName_, storeName: DontDestroyManager.StoreScene.storeName);
                break;
            case SceneName.StoreListScene:
                element = new SceneInfo(beforeSceneStr: sceneName_, categorySub: DontDestroyManager.StoreScene.categorySub);
                break;
            case SceneName.StoreScene:
                element = new SceneInfo(beforeSceneStr: sceneName_, storeName: DontDestroyManager.StoreScene.storeName, categorySub: DontDestroyManager.StoreScene.categorySub);
                break;
            default:
                break;
        }
        Stack.Instance.Push(element);
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
