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

    public static bool isLogin { get; set; } = false;
    public static User user;
    public static bool isAdvertise = false; //우리 매장 홍보하기를 누르고 로그인을 했나?

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
        if (!isLogin || storeName != user.storeName)
            return (false);
        return (true);
    }

}
