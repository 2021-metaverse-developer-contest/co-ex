using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectStoreItemClick : MonoBehaviour
{
    public void ListItemOnClick()
    {
        GameObject cur = EventSystem.current.currentSelectedGameObject;
        TMP_InputField search = GameObject.Find("InputTMP_SearchSelect").GetComponent<TMP_InputField>();
        search.text = cur.GetComponentInChildren<TextMeshProUGUI>().text;
        Button searchBtn = GameObject.Find("Btn_SearchSelect").GetComponent<Button>();
        searchBtn.onClick.Invoke();
	}

    public void ResultItemOnClick()
    {
        GameObject cur = EventSystem.current.currentSelectedGameObject;
        TMP_InputField storeField = GameObject.Find("Input_Store").GetComponent<TMP_InputField>();
        storeField.text = cur.transform.Find("TMP_Result").GetComponent<TextMeshProUGUI>().text;
        storeField.text += "(" + cur.transform.Find("TMP_Floor").GetComponent<TextMeshProUGUI>().text + ")";
        //SceneManager.UnloadSceneAsync("SelectStoreScene");
        GameObject.Find("Panel_SelectStore").SetActive(false);
    }

    public void CloseSelectStoreBtnOnClick()
    {
        GameObject.Find("Panel_SelectStore").SetActive(false);
        //SceneManager.UnloadSceneAsync("SelectStoreScene");
    }
}