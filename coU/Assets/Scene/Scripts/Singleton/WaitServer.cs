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
    public bool isDone = false;


    public IEnumerator waitServer()
    {
        SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        while (this.isDone == false)
        {
            yield return null;
        }
        this.isDone = false;
        SceneManager.UnloadSceneAsync("LoadingScene");
    }
    
}
