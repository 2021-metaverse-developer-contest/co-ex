using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginSceneManager : MonoBehaviour
{
    [SerializeField]
    TMP_InputField idField;
    [SerializeField]
    TMP_InputField pwField;

    // 인증을 관리할 객체
    Firebase.Auth.FirebaseAuth auth;

    private void Awake()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance; //객체 초기화
    }

	private void Start()
	{
        Screen.orientation = ScreenOrientation.Portrait;

    }

    public void login()
    {
        Debug.Log("Is in login?");
        auth.SignInWithEmailAndPasswordAsync(idField.text, pwField.text).ContinueWith(
            task =>
            {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log(idField.text + "로 로그인 하셨습니다.");
                    Toast.ShowToastMessage(idField.text + " 로그인 성공", 500);
                }
                else
                {
                    Debug.Log("로그인 실패");
                    Toast.ShowToastMessage(idField.text + " 로그인 실패\n회원가입 해주세요.", 500);
                }
            });
    }

    public void register()
    {
        Debug.Log("Is in register?");
        auth.CreateUserWithEmailAndPasswordAsync(idField.text, pwField.text).ContinueWith(
            task =>
            {
                if (!task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log(idField.text + "로 회원가입");
                    Toast.ShowToastMessage(idField.text + " 회원가입 성공", 500);
                }
                else
                {
                    Debug.Log("회원가입 실패");
                    Toast.ShowToastMessage(idField.text + " 회원가입 실패", 500);
                }
            });

    }
}
