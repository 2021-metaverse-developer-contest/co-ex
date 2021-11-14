using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginBtnClick : MonoBehaviour
{
	public void LoginCoroutine(string loginId, string loginPw)
	{
		StartCoroutine(Login(loginId, loginPw));
	}

	public IEnumerator Login(string loginId, string loginPw)
	{
		GameObject panelPop = GameObject.Find("Canvas_Pop").transform.Find("Panel_PopWhole").gameObject;
		TextMeshProUGUI tmpMsg = panelPop.transform.Find("Panel_Pop/TMP_Msg").GetComponent<TextMeshProUGUI>();
		FirebaseRealtimeManager.Instance.readUser(LoginSceneManager.GetKeyFromEmail(loginId));
		yield return WaitServer.Instance.waitServer();
		User loginUser = FirebaseRealtimeManager.Instance.user;
		if (loginUser == null)
		{
			Debug.Log("아이디를 확인해주세요.");
			panelPop.SetActive(true);
			tmpMsg.text = "아이디를 확인해주세요.";
		}
		else
		{
			Debug.Log("loginID: " + loginUser?.id);
			if (loginUser.id == loginId && loginUser.pw == loginPw)
			{
				Debug.Log($"{loginUser.id}님 안녕하세요."); // 여기서 출력된다.
				LoginSceneManager.user = loginUser;
				LoginSceneManager.isLogin = true;
#if UNITY_EDITOR
				Debug.Log(loginUser.id.Split('@')[0] + "으로 로그인했습니다.");
#elif UNITY_ANDROID
				Toast.ShowToastMessage(loginUser.id.Split('@')[0] + "으로 로그인했습니다.", 3000);
#endif
			}
			else
			{
				if (loginUser.id != loginId)
				{
					Debug.Log("아이디를 확인해주세요.");
					panelPop.SetActive(true);
					tmpMsg.text = "아이디를 확인해주세요.";
				}
				else
				{
					Debug.Log("비밀번호를 확인해주세요.");
					panelPop.SetActive(true);
					tmpMsg.text = "비밀번호가 일치하지않습니다.";
				}
			}
		}

		//
		if (LoginSceneManager.isLogin == true)
		{
			if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MaxstScene"))
				SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
			// 로그인이 된 다음에 씬전환
			if (LoginSceneManager.isAdvertise) // 1. 우리 매장 홍보하기를 통해 들어온건가?
				SceneManager.LoadSceneAsync("UploadScene", LoadSceneMode.Additive);
			//else // 2. 로그인 버튼을 통해 들어온건가?
			//	SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Additive);
			if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MaxstScene"))
				SceneManager.UnloadSceneAsync("LoginScene");
		}
		else
		{
			// 로그인이 되지 않았으니 아무런 동작도 하지 않음
		}
	}

	public void LoginBtnOnClick()
	{
		GameObject panelPop = GameObject.Find("Canvas_Pop").transform.Find("Panel_PopWhole").gameObject;
		TextMeshProUGUI tmpMsg = panelPop.transform.Find("Panel_Pop/TMP_Msg").GetComponent<TextMeshProUGUI>();
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
		else
		{
			LoginCoroutine(fieldID.text, fieldPW.text);
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
		SceneManager.UnloadSceneAsync("LoginScene");
	}

	public void CloseBtnOnClick()
	{
		SceneManager.UnloadSceneAsync("LoginScene");
	}
}
