using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInfo : MonoBehaviour
{
    public string sceneName { get; }
    public int sceneIndex { get; }
    public string storeName { get; }
    public string categorySub { get; }


    // yunslee 12.5
    public SceneInfo(string sceneName, int sceneIndex = -1, string storeName = "", string categorySub = "")
    {
        this.sceneName = sceneName;
        if (sceneIndex == -1)
            this.sceneIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
        else
            this.sceneIndex = sceneIndex;
        this.storeName = storeName;
        this.categorySub = categorySub;
    }
}
