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
        //AllCategoryScene의 이전은 언제나 MaxstScene이므로 스택에 push하지 않음.
    }

    public void StoreOnClick()
    {
        Debug.Log("Store Info Click");
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;

        TextMeshProUGUI storeName = clickObj.transform.Find("Panel_StoreInfo").GetComponentInChildren<TextMeshProUGUI>();
        //TextMeshProUGUI storeName = clickObj.transform.Find("Panel_StoreInfo").Find("TMP_StoreName").GetComponent<TextMeshProUGUI>();
        SceneManager.LoadScene("StoreScene");
        StoreSceneManager.storeName = storeName.text;
        Stack.Instance.Push(new SceneInfo(SceneManager.GetActiveScene().buildIndex, storeName.text, true));
        //StoreSceneManager.isMaxst = true;
    }
}
