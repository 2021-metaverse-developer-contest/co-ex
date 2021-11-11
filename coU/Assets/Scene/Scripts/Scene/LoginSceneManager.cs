using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginSceneManager : MonoBehaviour
{
    public static bool isLogin { get; set; } = false;
    public static User user;
    public static bool isAdvertise = false; //우리 매장 홍보하기를 누르고 로그인을 했나?


    // UploadScene에 접근할 때 사용함
    public static bool IsPermission(string storeName)
    {
        if (!isLogin || storeName != user.storeName)
            return (false);
        return (true);
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.UnloadSceneAsync("LoginScene");
	}
}
