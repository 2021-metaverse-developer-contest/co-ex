using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginBtnClick : MonoBehaviour
{
	public void LoginBtnOnClick()
	{
		GameObject panelPop = GameObject.Find("Canvas_Pop").transform.Find("Panel_PopWhole").gameObject;
		TextMeshProUGUI tmpMsg = GameObject.Find("TMP_Msg").GetComponent<TextMeshProUGUI>();
		TMP_InputField fieldID = GameObject.Find("Input_ID").GetComponent<TMP_InputField>();
		TMP_InputField fieldPW = GameObject.Find("Input_PW").GetComponent<TMP_InputField>();

		if (fieldID.text == "") // 1.id필드가 비어있다.
		{
			panelPop.SetActive(true);
			tmpMsg.text = "이메일을 입력해주세요.";
		}
		else if (fieldPW.text == "") // 2.pw필드가 비어있다.
		{
			panelPop.SetActive(true);
			tmpMsg.text = "비밀번호를 입력해주세요.";
		}
		else if (!(new Regex(@"^[\w.%+\-]+@[\w.\-]+\.[A-Za-z]{2,3}$").IsMatch(fieldID.text))) // 3. id의 형식이 이메일이 아니다.
		{
			panelPop.SetActive(true);
			tmpMsg.text = "올바른 이메일 형식이 아닙니다.\n다시 입력해주세요.";
		}
		else if (true) // 4. 가입이 되어있지 않다.
		{

		}
		else // => 로그인 성공!
		{
		}
	}

	public void PopOKBtnOnClick()
	{
		GameObject.Find("Panel_PopWhole").SetActive(false);
	}

	public void FindIdBtnOnClick()
	{
		// 회원가입 시 이메일과 비밀번호, 매장만 선택하기 때문에
		// 매장을 선택하면 이메일 힌트를 주는 게 제일 베스트일 것 같기도,,
		// ex) 42.4.hyojlee@gmail.com
		// => 42.4.hyo****@gmail.com
		// 이런 식으로?
	}

	public void FindPwBtnOnClick()
	{

	}

	public void RegisterBtnOnClick()
	{
		SceneManager.LoadSceneAsync("RegisterScene", LoadSceneMode.Additive);
	}

	public void CloseBtnOnClick()
	{
		SceneManager.UnloadSceneAsync("LoginScene");
	}
}
