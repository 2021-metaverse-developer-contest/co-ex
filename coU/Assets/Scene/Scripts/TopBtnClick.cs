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

    public void BackBtnOnClick()
    {
        string activeScene = SceneManager.GetActiveScene().name;

        if (activeScene.Contains("Search"))
            SceneManager.LoadScene(SearchSceneManager.beforeScene);
        else if (activeScene.Contains("StoreList"))
            SceneManager.LoadScene("AllCategoryScene");
        else if (activeScene.Contains("Store"))
        {
            if (StoreSceneManager.beforeScene)
                SceneManager.LoadScene("SearchScene");
            else
                SceneManager.LoadScene("StoreListScene");
        }
    }
}
