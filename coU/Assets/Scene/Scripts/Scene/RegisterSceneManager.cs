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
    }
}
