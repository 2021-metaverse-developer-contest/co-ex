using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SearchItemClick : MonoBehaviour
{
    public void ListItemOnClick()
    {
        GameObject cur = EventSystem.current.currentSelectedGameObject;
        TMP_InputField search = GameObject.Find("InputTMP_Search").GetComponent<TMP_InputField>();

        search.text = cur.GetComponentInChildren<TextMeshProUGUI>().text;
        Button searchBtn = GameObject.Find("Btn_Search").GetComponent<Button>();
        searchBtn.onClick.Invoke();
    }

    public void ResultItemOnClick()
    {
        GameObject cur = EventSystem.current.currentSelectedGameObject;
        
        StoreSceneManager.storeName = cur.transform.Find("TMP_Result").GetComponent<TextMeshProUGUI>().text;
        StoreSceneManager.categorySub = "";
        Stack.Instance.Push(new SceneInfo(SceneManager.GetActiveScene().buildIndex, SearchSceneManager.searchStr, true));
        SceneManager.LoadScene("StoreScene");
    }
}