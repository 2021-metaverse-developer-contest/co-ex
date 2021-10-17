using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitBtnClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitBtnOnClick()
    {
#if (UNITY_EDITOR)
    UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_ANDROID && !UNITY_EDITOR)
    Application.Quit();
#elif (UNITY_IOS && !UNITY_EDITOR)
    Application.Quit();
#endif
    }

    public void CancelBtnOnClick()
    {
        CategorySceneManager.popUpInActive();
    }
}
