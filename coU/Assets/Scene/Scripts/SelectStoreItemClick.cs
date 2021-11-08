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
        TMP_InputField search = GameObject.Find("InputTMP_Search").GetComponent<TMP_InputField>();
        search.text = cur.GetComponentInChildren<TextMeshProUGUI>().text;
        Button searchBtn = GameObject.Find("Btn_Search").GetComponent<Button>();
        searchBtn.onClick.Invoke();

		//StoreSceneManager.storeName = cur.transform.Find("TMP_List").GetComponent<TextMeshProUGUI>().text;
		//StoreSceneManager.categorySub = "";
		//Stack.Instance.Push(new SceneInfo(SceneManager.GetActiveScene().buildIndex, StoreSceneManager.storeName, true));
		//SceneManager.LoadScene("StoreScene");
	}

    public void ResultItemOnClick()
    {
        GameObject cur = EventSystem.current.currentSelectedGameObject;
        RegisterSceneManager.storeField.text = cur.transform.Find("TMP_Result").GetComponent<TextMeshProUGUI>().text;
        RegisterSceneManager.storeField.text += "(" + cur.transform.Find("TMP_Floor").GetComponent<TextMeshProUGUI>().text + ")";
        SceneManager.UnloadSceneAsync("SelectStoreScene");
        //StoreSceneManager.storeName = cur.transform.Find("TMP_Result").GetComponent<TextMeshProUGUI>().text;
        //StoreSceneManager.categorySub = "";
        //Stack.Instance.Push(new SceneInfo(SceneManager.GetActiveScene().buildIndex, SearchSceneManager.searchStr, true));
        //SceneManager.LoadScene("StoreScene");
    }

    public void CloseBtnOnClick()
    {
        SceneManager.UnloadSceneAsync("SelectStoreScene");
    }
}