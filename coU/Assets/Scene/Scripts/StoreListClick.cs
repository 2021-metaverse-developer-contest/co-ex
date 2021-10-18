using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StoreListClick : MonoBehaviour
{
    public static string clickFloor = "";
    public static void FloorBtnOnClick()
    {
        GameObject click = EventSystem.current.currentSelectedGameObject;
        if (click != null)
            clickFloor = EventSystem.current.currentSelectedGameObject.name;
        GameObject parent = GameObject.Find("Panel_List").gameObject;
        GameObject F1Content = parent.transform.Find("ScrollView_F1").gameObject;
        GameObject B1Content = parent.transform.Find("ScrollView_B1").gameObject;
        GameObject B2Content = parent.transform.Find("ScrollView_B2").gameObject;

        if (clickFloor.Contains("F1"))
        {
            F1Content.SetActive(true);
            B1Content.SetActive(false);
            B2Content.SetActive(false);
            clickFloor = "F1";
        }
        else if (clickFloor.Contains("B1"))
        {
            F1Content.SetActive(false);
            B1Content.SetActive(true);
            B2Content.SetActive(false);
            clickFloor = "B1";
        }
        else if (clickFloor.Contains("B2"))
        {
            F1Content.SetActive(false);
            B1Content.SetActive(false);
            B2Content.SetActive(true);
            clickFloor = "B2";
        }
        else
            Debug.Log("Click Error");
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Btn_" + clickFloor).gameObject);
    }

    public void StoreOnClick()
    {
        GameObject click = EventSystem.current.currentSelectedGameObject;
        string storeName = click.transform.Find("Panel_Name").GetComponentInChildren<TextMeshProUGUI>().text;
        //string categorySub = GameObject.Find("TMP_SubCategory").GetComponent<TextMeshProUGUI>().text;
        string categorySub = StoreListSceneManager.categorySub;

        SceneManager.LoadScene("StoreScene");
        StoreSceneManager.storeName = storeName;
        StoreSceneManager.categorySub = categorySub;
        StoreSceneManager.beforeScene = true;
    }
}
