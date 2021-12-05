using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginSceneManager : MonoBehaviour
{
    // 임시로 로그인 정보 담아둠
    [SerializeField]
    private TMP_InputField idField;
    [SerializeField]
    private TMP_InputField pwField;

    // 임시로 로그인 정보 담아둠.
	private void Start()
	{
		
	}


	public static string GetKeyFromEmail(string email)
    {
        return email.Replace('.', '_').Replace('@', '_');
        //string[] ids = email.Split('@');
        //return (ids[0] + "_" + ids[1].Split('.')[0]).Replace('.', '_');
    }

    // UploadScene에 접근할 때 사용함
    public static bool IsPermission(string storeName)
    {
        if (!DontDestroyManager.LoginScene.isLogin || storeName != DontDestroyManager.LoginScene.user.storeName)
            return (false);
        return (true);
    }
}
