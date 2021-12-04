using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyManager : MonoBehaviour
{
    static public DontDestroyData.LoginSceneData LoginScene;


    void Awake()
    {
        if (FindObjectsOfType<DontDestroyManager>().Length > 0)
		{
            DontDestroyData ee = new DontDestroyData();
			DontDestroyOnLoad(this);
		}
		else
			Destroy(this);
	}
    void Start()
    {
        LoginScene = new DontDestroyData.LoginSceneData();
    }
}
