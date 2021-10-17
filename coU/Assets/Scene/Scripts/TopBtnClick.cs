using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopBtnClick : MonoBehaviour
{
    public void HomeBtnOnClick()
    {
        SceneManager.LoadScene("MaxstScene");
    }

    public void SearchBtnOnClick()
    {
        SceneManager.LoadScene("SearchScene");
        SearchSceneManager.beforeScene = SceneManager.GetActiveScene().buildIndex;
    }
}
