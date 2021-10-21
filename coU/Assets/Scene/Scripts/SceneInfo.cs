using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfo : MonoBehaviour
{
    public int beforeScene { get; }
    public string storeName { get; }
    public string categorySub { get; }

    public SceneInfo(int beforeScene)
    {
        this.beforeScene = beforeScene;
        this.storeName = "";
        this.categorySub = "";
    }

    public SceneInfo(int beforeScene, string str, bool isName)
    {
        this.beforeScene = beforeScene;
        if (isName)
        {
            this.storeName = str;
            this.categorySub = "";
        }
        else
        {
            this.storeName = "";
            this.categorySub = str;
        }
    }

    public SceneInfo(int beforeScene, string storeName, string categorySub)
    {
        this.beforeScene = beforeScene;
        this.storeName = storeName;
        this.categorySub = categorySub;
    }
}
