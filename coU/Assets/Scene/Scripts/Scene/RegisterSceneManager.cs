using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegisterSceneManager : MonoBehaviour
{
    public static TMP_InputField storeField;

	// Start is called before the first frame update
	void Start()
    {
        storeField = GameObject.Find("Input_Store").GetComponent<TMP_InputField>();
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
