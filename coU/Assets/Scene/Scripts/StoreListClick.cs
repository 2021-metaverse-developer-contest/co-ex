using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StoreListClick : MonoBehaviour
{
    public void FloorBtnOnClick()
    {
        GameObject click = EventSystem.current.currentSelectedGameObject;
        GameObject parent = GameObject.Find("Panel_List").gameObject;
        GameObject F1Content = parent.transform.Find("ScrollView_F1").gameObject;
        GameObject B1Content = parent.transform.Find("ScrollView_B1").gameObject;
        GameObject B2Content = parent.transform.Find("ScrollView_B2").gameObject;

        if (click.name.Contains("F1"))
        {
            F1Content.SetActive(true);
            B1Content.SetActive(false);
            B2Content.SetActive(false);
        }
        else if (click.name.Contains("B1"))
        {
            F1Content.SetActive(false);
            B1Content.SetActive(true);
            B2Content.SetActive(false);
        }
        else if (click.name.Contains("B2"))
        {
            F1Content.SetActive(false);
            B1Content.SetActive(false);
            B2Content.SetActive(true);
        }
        else
            Debug.Log("Click Error");
    }

    public void StoreOnClick()
    {
        GameObject click = EventSystem.current.currentSelectedGameObject;
        string storeName = click.transform.Find("Panel_Name").GetComponentInChildren<TextMeshProUGUI>().text;
        string categorySub = GameObject.Find("TMP_SubCategory").GetComponent<TextMeshProUGUI>().text;

        SceneManager.LoadScene("StoreScene");
        StoreSceneManager.storeName = storeName;
        StoreSceneManager.categorySub = categorySub;
        StoreSceneManager.beforeScene = true;
    }
}
