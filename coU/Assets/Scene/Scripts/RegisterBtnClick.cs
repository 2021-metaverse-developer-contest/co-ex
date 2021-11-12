using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterBtnClick : MonoBehaviour
{
	static bool isDupChk = false; //중복확인 버튼을 클릭했는가?
	static bool isDupEmail = false; //이미 가입된 이메일인가?
	static bool isDone = false;

	void ChkDupEmailCoroutine(string email)
	{
		StartCoroutine(ChkDupEmail(email));
	}

	IEnumerator ChkDupEmail(string email)
	{
		string[] id = email.Split('@');
		FirebaseRealtimeManager.Instance.readValue<User>((id[0] + "_" + id[1].Split('.')[0]).Replace('.', '_'));
		yield return WaitServer.Instance.waitServer();
		if (FirebaseRealtimeManager.Instance.user == null)
			isDupEmail = false;
		else
			isDupEmail = true;
	}

	void RegisterCoroutine(string id, string pw, string storeName)
	{
		StartCoroutine(Register(id, pw, storeName));
	}

	IEnumerator Register(string id, string pw, string storeName)
	{
		User newUser = new User(id, pw, storeName, 0);
		string[] idList = newUser.id.Split('@');
		FirebaseRealtimeManager.Instance.createValue<User>((idList[0] + "_" + idList[1].Split('.')[0]).Replace('.', '_'), newUser);
		yield return WaitServer.Instance.waitServer();
	}

	bool ChkCorrectPw()
	{
		TMP_InputField fieldPw1 = GameObject.Find("Input_PW1").GetComponent<TMP_InputField>();
		TMP_InputField fieldPw2 = GameObject.Find("Input_PW2").GetComponent<TMP_InputField>();

		if (fieldPw1.text == fieldPw2.text)
			return true;
		return false;
	}

	public void ChkDupBtnOnClick()
	{
		TMP_InputField fieldID = GameObject.Find("Input_ID").GetComponent<TMP_InputField>();
		Transform popCanvas = GameObject.Find("Canvas_PopRegister").transform;
		TextMeshProUGUI errMsg = popCanvas.Find("Panel_PopErrorRegister/Panel_PopErrorReg/TMP_ErrMsg").GetComponent<TextMeshProUGUI>();

		Debug.Log("이메일 중복 체크 함수");
		// 1. 올바른 이메일 형식인가?
		if (!(new Regex(@"^[\w.%+\-]+@[\w.\-]+\.[A-Za-z]{2,3}$").IsMatch(fieldID.text)))
		{
			popCanvas.Find("Panel_PopErrorRegister").gameObject.SetActive(true);
			errMsg.text = "올바른 이메일 형식이 아닙니다.\n다시 입력하세요.";
			return;
		}
		popCanvas.Find("Panel_PopErrorRegister").gameObject.SetActive(true);

		ChkDupEmailCoroutine(fieldID.text);
		if (isDupEmail)
		{
			errMsg.text = "이미 가입된 이메일입니다.";
			fieldID.text = "";
			isDupChk = false;
		}
		else
		{
			errMsg.text = "사용 가능한 이메일입니다.";
			isDupChk = true;
		}
	}

	public void RegisterBtnOnClick()
	{
		TMP_InputField idField = GameObject.Find("Input_ID").GetComponent<TMP_InputField>();
		TMP_InputField pwField1 = GameObject.Find("Input_PW1").GetComponent<TMP_InputField>();
		TMP_InputField pwField2 = GameObject.Find("Input_PW2").GetComponent<TMP_InputField>();
		TMP_InputField storeField = GameObject.Find("Input_Store").GetComponent<TMP_InputField>();
		Transform popCanvas = GameObject.Find("Canvas_PopRegister").transform;
		TextMeshProUGUI errMsg = popCanvas.Find("Panel_PopErrorRegister/Panel_PopErrorReg/TMP_ErrMsg").GetComponent<TextMeshProUGUI>();

		Debug.Log("회원가입 클릭");

		/*
		 * 1. 모든 인풋필드가 입력되었는가?
		 * 2. 이메일 중복 확인을 하였는가?
		 * 3. 비밀번호가 일치하는가?
		 * 4. 이미 매장의 점주가 존재하는가? -> 일단 배제
		 */
		popCanvas.Find("Panel_PopErrorRegister").gameObject.SetActive(true);
		if (idField.text == "")
			errMsg.text = "이메일을 입력해주세요.";
		else if (pwField1.text == "")
			errMsg.text = "비밀번호를 입력해주세요.";
		else if (pwField2.text == "")
			errMsg.text = "비밀번호를 한 번 더 입력해주세요.";
		else if (storeField.text == "")
			errMsg.text = "매장을 선택해주세요.";
		else if (!isDupChk)
			errMsg.text = "이메일 중복 확인을 해주세요.";
		else if (!ChkCorrectPw())
			errMsg.text = "비밀번호가 일치하지 않습니다.";
		else
		{
			RegisterCoroutine(idField.text, pwField1.text, storeField.text.Substring(0, storeField.text.Length - 4));
			errMsg.text = "가입이 완료되었습니다.";
			isDone = true;
		}
	}

	public void CloseBtnOnClick() //Top에 있는 닫기 버튼 클릭 이벤트
	{
		Debug.Log("창 닫기");
		GameObject.Find("Canvas_PopRegister").transform.Find("Panel_PopCloseRegister").gameObject.SetActive(true);
	}

	public void CloseCancelBtnOnClick() //Panel_PopCloseRegister에 있는 취소 버튼 클릭 이벤트
	{
		GameObject.Find("Panel_PopCloseRegister").SetActive(false);
	}

	public void CloseOKBtnOnClick() //Panel_PopCloseRegister에 있는 확인 버튼 클릭 이벤트
	{
		SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync("RegisterScene");
	}

	public void PopErrOKBtnOnClick() //Panel_PopErrorRegister에 있는 확인 버튼 클릭 이벤틏
	{
		GameObject.Find("Panel_PopErrorRegister").SetActive(false);
		if (isDone)
		{
			SceneManager.UnloadSceneAsync("RegisterScene");
			SceneManager.LoadSceneAsync("LoginScene", LoadSceneMode.Additive);
			isDone = false;
		}

	}

	public void InputIDOnValueChanged()
	{
		isDupChk = false;
	}

}
