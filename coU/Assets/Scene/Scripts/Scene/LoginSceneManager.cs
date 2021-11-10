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
    public string loginId;
    public string loginPw;
    public string storeName;

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
        string[] id = loginId.Split('@');
        FirebaseRealtimeManager.Instance.readValue<User>((id[0] + "_" + id[1].Split('.')[0]).Replace('.', '_'));
        yield return WaitServer.Instance.waitServer();
        User loginUser = FirebaseRealtimeManager.Instance.user;
        if (loginUser == null)
		{
            Debug.Log("아이디를 확인해주세요.");

        }
        else
		{
            Debug.Log("loginID: " + loginUser?.id);
            if (loginUser.id == loginId && loginUser.pw == loginPw)
            {
                Debug.Log($"{loginUser.id}님 안녕하세요."); // 여기서 출력된다.
                LoginSceneManager.user = loginUser;
                LoginSceneManager.isLogin = true;
            }
            else
            {
                if (loginUser.id != loginId)
                    Debug.Log("아이디를 확인해주세요.");
                else
                    Debug.Log("비밀번호를 확인해주세요.");
            }
		}

        //
        if (LoginSceneManager.isLogin == true)
		{
            // 로그인이 된 다음에 씬전환
		}
        else
		{
            // 로그인이 되지 않았으니 아무런 동작도 하지 않음
		}
    }

    public IEnumerator Register()
    {
        User newUser = new User(loginId, loginPw, storeName, 0);
        FirebaseRealtimeManager.Instance.readValue<User>(newUser.id);
        yield return WaitServer.Instance.waitServer();
        User existUser = FirebaseRealtimeManager.Instance.user;
        if (existUser != null)
            Debug.Log("중복된 id가 있습니다.");
        else
        {
            //FirebaseRealtimeManager.Instance.createValue<User>(newUser.id, newUser);
            string[] id = newUser.id.Split('@');
            FirebaseRealtimeManager.Instance.createValue<User>((id[0] + "_" + id[1].Split('.')[0]).Replace('.', '_'), newUser);
            yield return WaitServer.Instance.waitServer();
            Debug.Log($"{newUser.id}가 등록되었습니다.");
        }
    }

    // UploadScene에 접근할 때 사용함
    public static bool IsPermission(string storeName)
    {
        if (!isLogin || storeName != user.storeName)
            return (false);
        return (true);
    }

    public static void Logout()
    {
        LoginSceneManager.isLogin = false;
        LoginSceneManager.user = null;
    }
}
