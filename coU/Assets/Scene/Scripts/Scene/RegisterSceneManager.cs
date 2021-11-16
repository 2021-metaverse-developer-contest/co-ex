using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegisterSceneManager : MonoBehaviour
{
    public static User user;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        user = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            if (GameObject.Find("Panel_SelectStore") != null)
                GameObject.Find("Panel_SelectStore").SetActive(false);
            else
                GameObject.Find("Canvas_PopRegister").transform.Find("Panel_PopCloseRegister").gameObject.SetActive(true);
		}
    }
}
