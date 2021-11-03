using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.UnloadSceneAsync("MenuScene");
    }
}
