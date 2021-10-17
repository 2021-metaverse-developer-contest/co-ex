using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubCtBtnClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        SceneManager.LoadScene("StoreListScene");
        StoreListSceneManager.categorySub = clickObject.GetComponentInChildren<TextMeshProUGUI>().text;
    }
}
