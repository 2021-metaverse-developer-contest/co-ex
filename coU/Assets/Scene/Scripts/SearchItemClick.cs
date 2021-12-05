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

        DontDestroyManager.StoreScene.storeName = cur.transform.Find("TMP_Result").GetComponent<TextMeshProUGUI>().text;
        DontDestroyManager.StoreScene.categorySub = "";
        DontDestroyManager.newPush(sceneName_: DontDestroyManager.getSceneName(EventSystem.current), storeName_: DontDestroyManager.SearchScene.searchStr);
        SceneManager.LoadScene("StoreScene");
    }
}