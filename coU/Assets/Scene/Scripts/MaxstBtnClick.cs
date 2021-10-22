using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MaxstBtnClick : MonoBehaviour
{
    public void SearchBtnOnClick()
    {
        SceneManager.LoadScene("SearchScene");
        Stack.Instance.Push(new SceneInfo(SceneManager.GetActiveScene().buildIndex));
    }

    public void CategoryBtnOnClick()
    {
        SceneManager.LoadScene("AllCategoryScene");
    }

    public void StoreOnClick()
    {
        Debug.Log("Store Info Click");
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;

        TextMeshProUGUI storeName = clickObj.transform.Find("Panel_StoreInfo").GetComponentInChildren<TextMeshProUGUI>();
        SceneManager.LoadScene("StoreScene");
        StoreSceneManager.storeName = storeName.text;
        StoreSceneManager.categorySub = "";
        Stack.Instance.Push(new SceneInfo(SceneManager.GetActiveScene().buildIndex));
    }
}
