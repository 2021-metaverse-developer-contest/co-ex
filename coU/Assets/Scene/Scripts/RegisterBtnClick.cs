using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterBtnClick : MonoBehaviour
{
	private bool isDupChk = false; //중복확인 버튼을 클릭했는가?
	private bool isDupEmail = false; //이미 가입된 이메일인가?
	private bool isDone = false;

	void ChkDupEmailCoroutine(TMP_InputField idField, TextMeshProUGUI txtMsg)
	{
		StartCoroutine(ChkDupEmail(idField, txtMsg));
	}

	IEnumerator ChkDupEmail(TMP_InputField idField, TextMeshProUGUI txtMsg)
	{
		WaitServer wait = new WaitServer();
		FirebaseRealtimeManager firebaseRealtime = new FirebaseRealtimeManager();
		firebaseRealtime.readUser(LoginSceneManager.GetKeyFromEmail(idField.text), wait);
		yield return wait.waitServer();
		if (firebaseRealtime.user == null)
		{
			Debug.Log("No Duplication");
			txtMsg.text = "사용 가능한 이메일입니다.";
			isDupEmail = false;
			isDupChk = true;
		}
		else
		{
			Debug.Log("Duplication");
			txtMsg.text = "이미 가입된 이메일입니다.";
			isDupEmail = true;
			isDupChk = false;
		}
	}

	void RegisterCoroutine(string id, string pw, string storeName, TextMeshProUGUI errMsg)
	{
		StartCoroutine(Register(id, pw, storeName, errMsg));
	}

	IEnumerator Register(string id, string pw, string storeName, TextMeshProUGUI errMsg)
	{
		User newUser = new User(id, pw, storeName, 0);
		WaitServer wait = new WaitServer();
		new FirebaseRealtimeManager().createUser(LoginSceneManager.GetKeyFromEmail(newUser.id), newUser, wait);
		yield return wait.waitServer();
		errMsg.text = "가입이 완료되었습니다.";
		isDone = true;
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

		ChkDupEmailCoroutine(fieldID, errMsg);
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
			RegisterCoroutine(idField.text, pwField1.text
				, storeField.text.Substring(0, storeField.text.LastIndexOf("(")), errMsg);
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
		SceneManager.LoadScene("MenuScene", LoadSceneMode.Additive);
		SceneManager.UnloadScene("RegisterScene");
	}

	public void PopErrOKBtnOnClick() //Panel_PopErrorRegister에 있는 확인 버튼 클릭 이벤틏
	{
		GameObject.Find("Panel_PopErrorRegister").SetActive(false);
		if (isDone)
		{
			SceneManager.UnloadScene("RegisterScene");
			SceneManager.LoadScene("LoginScene", LoadSceneMode.Additive);
			isDone = false;
		}

	}

	public void InputIDOnValueChanged()
	{
		isDupChk = false;
	}

}
