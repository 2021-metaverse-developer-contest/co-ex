using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MaxstBtnClick : MonoBehaviour
{
    //public void SearchBtnOnClick()
    //{
    //    SceneManager.LoadScene("SearchScene");
    //    Stack.Instance.Push(new SceneInfo(SceneManager.GetActiveScene().buildIndex));
    //}

    //public void MenuBtnOnClick()
    //{
    //    //SceneManager.LoadScene("AllCategoryScene");
    //    SceneManager.LoadScene("MenuScene", LoadSceneMode.Additive);
    //}

    public void StoreOnClick()
    {
        Debug.Log("Store Info Click");
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;

        TextMeshProUGUI storeName = clickObj.transform.Find("Panel_StoreInfoParent/Panel_StoreInfo").GetComponentInChildren<TextMeshProUGUI>();
        SceneManager.LoadScene("StoreScene", LoadSceneMode.Additive);
        DontDestroyManager.StoreScene.storeName = storeName.text;
        DontDestroyManager.StoreScene.categorySub = "";
        DontDestroyManager.newPush(new SceneInfo(beforeSceneStr: DontDestroyManager.getSceneName(EventSystem.current)));
    }

    public void NaviQuitBtnOnClick()
    {
        Debug.Log("NaviQuitBtn Click");
        GameObject popUp = GameObject.Find("Canvas_Navi").transform.Find("Panel_PopUp").gameObject;

        popUp.SetActive(true);
    }

    public void PopUpQuitBtnOnClick()
    {
        Debug.Log("Quit Navigation");
        GameObject.Find("Panel_PopUp").SetActive(false);
        MaxstSceneManager.ActivePanelChange();
    }

    public void PopUpContinueBtnOnClick()
    {
        Debug.Log("Navigation Continue");
        GameObject.Find("Panel_PopUp").SetActive(false);
    }

    public void ArrivalQuitOnClick()
    {
        Debug.Log("Arrival Pop up Quit");
        MaxstSceneManager.ActivePanelChange();
    }

}
