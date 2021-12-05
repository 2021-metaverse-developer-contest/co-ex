using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInfo : MonoBehaviour
{
    public int beforeScene { get; }
    public string beforeSceneStr { get; }
    public string storeName { get; }
    public string categorySub { get; }


    // yunslee 12.5
    public SceneInfo(string beforeSceneStr, int beforeScene = -1, string storeName = "", string categorySub = "")
    {
        this.beforeSceneStr = beforeSceneStr;
        print($"beforeSceneStr: {beforeSceneStr}");
        if (beforeScene == -1)
            this.beforeScene = SceneUtility.GetBuildIndexByScenePath(beforeSceneStr);
        else
            this.beforeScene = beforeScene;
        print($"SceneInfo:: {this.beforeScene}:{beforeSceneStr + ".unity"}");
        this.storeName = storeName;
        this.categorySub = categorySub;
    }
    
    /* Leagcy */
    //public SceneInfo(int beforeScene)
    //{
    //    this.beforeScene = beforeScene;
    //    this.storeName = "";
    //    this.categorySub = "";
    //}

    //public SceneInfo(int beforeScene, string str, bool isName)
    //{
    //    this.beforeScene = beforeScene;
    //    if (isName)
    //    {
    //        this.storeName = str;
    //        this.categorySub = "";
    //    }
    //    else
    //    {
    //        this.storeName = "";
    //        this.categorySub = str;
    //    }
    //}

    //public SceneInfo(int beforeScene, string storeName, string categorySub)
    //{
    //    this.beforeScene = beforeScene;
    //    this.storeName = storeName;
    //    this.categorySub = categorySub;
    //}
}
