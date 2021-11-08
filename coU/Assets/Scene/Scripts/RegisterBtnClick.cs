using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterBtnClick : MonoBehaviour
{
	public void ChkDupBtnOnClick()
	{
		TMP_InputField fieldID = GameObject.Find("Input_ID").GetComponent<TMP_InputField>();

		Debug.Log("이메일 중복 체크 함수");
		if (ChkDupEmail())
		{
			// Pop 띄우던지, Toast 메시지 보내던지
			// => 이미 가입되어있습니다!
			// => 다른 이메일 사용하라고 안내? 아니면 id필드만 비워주면 사용자가 알아서 한다는 전제?
			fieldID.text = "";
		}
	}

	public void RegisterBtnOnClick()
	{
		Debug.Log("회원가입 클릭");
		/*
		 * 1. 모든 인풋필드가 입력되었는가?
		 * 2. 이메일 형식이 올바른가?
		 * 3. 비밀번호가 일치하는가?
		 * 4. 이미 매장의 점주가 존재하는가?
		 */
	}

	public void CloseBtnOnClick()
	{
		Debug.Log("창 닫기");
		GameObject.Find("Canvas_Pop").transform.Find("Panel_PopClose").gameObject.SetActive(true);
	}

	public void CloseCancelBtnOnClick()
	{
		GameObject.Find("Panel_PopClose").SetActive(false);
	}

	public void CloseOKBtnOnClick()
	{
		// 회원가입을 취소했다 == 로그인도 하지 못한다.
		// => 회원가입 씬과 로그인 씬 모두 Unload한다
		SceneManager.UnloadSceneAsync("RegisterScene");
		SceneManager.UnloadSceneAsync("LoginScene");
	}

	bool ChkCorrectPw()
	{
		TMP_InputField fieldPw1 = GameObject.Find("Input_PW1").GetComponent<TMP_InputField>();
		TMP_InputField fieldPw2 = GameObject.Find("Input_PW2").GetComponent<TMP_InputField>();

		if (fieldPw1.text == fieldPw2.text)
			return true;
		return false;
	}

	bool ChkDupEmail()
	{
		string email = GameObject.Find("Input_ID").GetComponent<TMP_InputField>().text;
		string query = "Select * from Users Where id = '" + email + "';";

		return false;
	}
}
