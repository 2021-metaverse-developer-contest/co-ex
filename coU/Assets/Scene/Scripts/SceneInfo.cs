using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfo : MonoBehaviour
{
    private int beforeScene;
    private string storeName;
    private string categoryMain;
    private string categorySub;
    private string floor;

    SceneInfo(int beforeScene, string storeName, string categoryMain, string categorySub)
    {
        this.beforeScene = beforeScene;
        this.storeName = storeName;
        this.categoryMain = categoryMain;
        this.categorySub = categorySub;
        this.floor = null;
    }

    SceneInfo(int beforeScene, string storeName, string categoryMain, string categorySub, string floor)
    {
        this.beforeScene = beforeScene;
        this.storeName = storeName;
        this.categoryMain = categoryMain;
        this.categorySub = categorySub;
        this.floor = floor;
    }
}
