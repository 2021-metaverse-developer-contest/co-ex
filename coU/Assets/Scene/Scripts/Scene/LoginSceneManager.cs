using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginSceneManager : MonoBehaviour
{
    public static bool isDone = false;
    public static bool isLogin { get; set; } = false;
    public static User user;
    public string loginId;
    public string loginPw;
    public string storeName;


    IEnumerator transactionDelay()
    {
        SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        while (LoginSceneManager.isDone == false)
        {
            yield return null;
        }
        LoginSceneManager.isDone = false;
        SceneManager.UnloadSceneAsync("LoadingScene");
    }

    public void LoginCorutine()
    {
        StartCoroutine(Login());
    }

    public void RegisterCorutine()
    {
        StartCoroutine(Register());
    }

    public IEnumerator Login()
    {
        FirebaseRealtimeManager.Instance.readValue<User>(loginId);
        yield return transactionDelay();
        User loginUser = FirebaseRealtimeManager.Instance.user;
        Debug.Log("loginUser " + loginUser.id);
        if (loginUser.id == loginId && loginUser.pw == loginPw)
        {
            Debug.Log($"{loginUser.id}:{loginUser.pw} 로그인!"); // 여기서 출력된다.
            LoginSceneManager.user = loginUser;
            LoginSceneManager.isLogin = true;
        }
        else
        {
            Debug.Log($"아이디 혹은 비밀번호를 확인해주세요");
            LoginSceneManager.user = null;
        }
    }

    public IEnumerator Register()
    {
        User newUser = new User(loginId, loginPw, storeName, 0);
        FirebaseRealtimeManager.Instance.readValue<User>(newUser.id);
        yield return transactionDelay();
        User existUser = FirebaseRealtimeManager.Instance.user;
        if (existUser != null)
            Debug.Log("중복된 id가 있습니다.");
        else
        {
            FirebaseRealtimeManager.Instance.createValue<User>(newUser.id, newUser);
            StartCoroutine(transactionDelay());
            Debug.Log($"{newUser.id}가 등록되었습니다.");
        }
    }

    // UploadScene에 접근할 때 사용함
    public bool IsPermission(string storeName)
    {
        if (!LoginSceneManager.isLogin || storeName != user.storeName)
            return (false);
        return (true);
    }

    public void Logout()
    {
        LoginSceneManager.isLogin = false;
        LoginSceneManager.user = null;
    }
}
