using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreListClick : MonoBehaviour
{
    public static string clickFloor = "";

    public static void FloorBtnOnClick()
    {
        GameObject click = EventSystem.current.currentSelectedGameObject;
        if (click != null)
            clickFloor = EventSystem.current.currentSelectedGameObject.name;
        else
            click = GameObject.Find("Btn_" + clickFloor).gameObject;
        GameObject parent = GameObject.Find("Panel_List").gameObject;
        GameObject F1Content = parent.transform.Find("ScrollView_F1").gameObject;
        GameObject B1Content = parent.transform.Find("ScrollView_B1").gameObject;
        GameObject B2Content = parent.transform.Find("ScrollView_B2").gameObject;

        click.GetComponent<Image>().color = new Color32(198, 215, 255, 76);

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
        ChangeBtnColor(clickFloor);

    }

    private static void ChangeBtnColor(string selectFloor)
    {
        GameObject b1 = GameObject.Find("Btn_B1");
        GameObject b2 = GameObject.Find("Btn_B2");
        GameObject f1 = GameObject.Find("Btn_F1");

        if (selectFloor == "F1")
        {
            if (b1 != null)
                b1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            if (b2 != null)
                b2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else if (selectFloor == "B1")
        {
            if (f1 != null)
                f1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            if (b2 != null)
                b2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            if (f1 != null)
                f1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            if (b1 != null)
                b1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

    }

    public void StoreOnClick()
    {
        GameObject click = EventSystem.current.currentSelectedGameObject;
        string storeName = click.transform.Find("Panel_Name").GetComponentInChildren<TextMeshProUGUI>().text;
        string categorySub = DontDestroyManager.StoreListScene.categorySub;

        DontDestroyManager.StoreScene.storeName = storeName;
        DontDestroyManager.StoreScene.categorySub = categorySub;
        DontDestroyManager.newPush(sceneName_: DontDestroyManager.getSceneName(EventSystem.current), storeName_:storeName, categorySub_:categorySub);
        SceneManager.LoadScene("StoreScene");
    }
}
