using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitServer
{
    //private static WaitServer _waitServer = null;
    //private WaitServer() { }
    //public static WaitServer Instance
    //{
    //    get
    //    {
    //        if (_waitServer == null)
    //            _waitServer = new WaitServer();
    //        return _waitServer;
    //    }
    //}

    public WaitServer()
    {
        this.isDone = false;
        this.isLoadScene = true;
    }
    public WaitServer(bool isLoadScene)
    {
        this.isDone = false;
        this.isLoadScene = isLoadScene;
    }

    public bool isDone;
    public bool isLoadScene;

    public IEnumerator waitServer()
    {
        if (this.isLoadScene == true)
        {
            SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);

            //int index = SceneManager.sceneCount;
            //var op = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
            //yield return new WaitUntil(() => op.isDone);
            //Scene loadedScene = SceneManager.GetSceneAt(index);

            Debug.Log("waitServer Start");
            while (this.isDone == false)
            {
                //Debug.Log("waitServer ~ing");
                yield return null;
            }
            this.isDone = false;
            Debug.Log("waitServer End");

			//SceneManager.UnloadSceneAsync(loadedScene);

			SceneManager.UnloadSceneAsync("LoadingScene");
        }
        else
        {
            //Debug.Log("waitServer Start");
            while (this.isDone == false)
            {
                //Debug.Log("waitServer ~ing");
                yield return null;
            }
            this.isDone = false;
            //Debug.Log("waitServer End");
        }
    }
}
