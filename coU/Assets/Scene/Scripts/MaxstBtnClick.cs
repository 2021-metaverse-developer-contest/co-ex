using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MaxstBtnClick : MonoBehaviour
{
    public void SearchBtnOnClick()
    {
        SceneManager.LoadScene("SearchScene");
        SearchSceneManager.beforeScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void CategoryBtnOnClick()
    {
        SceneManager.LoadScene("AllCategoryScene");
    }
}
