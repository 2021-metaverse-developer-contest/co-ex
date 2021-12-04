using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        GameObject panelLoginParent = GameObject.Find("Panel_MenuScene");
        if (DontDestroyManager.LoginScene.isLogin)
        {
            panelLoginParent.transform.Find("Panel_Login").gameObject.SetActive(false);
            Transform panelLogout = panelLoginParent.transform.Find("Panel_Logout");
            panelLogout.Find("Panel_User/TMP_User").GetComponent<TextMeshProUGUI>().text = DontDestroyManager.LoginScene.user.id.Split('@')[0] + "님";
            panelLogout.gameObject.SetActive(true);
        }
        else
        {
            panelLoginParent.transform.Find("Panel_Login").gameObject.SetActive(true);
            panelLoginParent.transform.Find("Panel_Logout").gameObject.SetActive(false);
        }
    }

    void Update()
    {
    }
}
