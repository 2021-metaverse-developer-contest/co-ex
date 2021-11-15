using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegisterSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        notDuplicatedId = false;
        user = null;
    }

    public static User user;
    public string registerId;
    public string registerPw;
    public string registerPw2;
    public string storeName;
    private bool notDuplicatedId;

    public bool checkPasswordCorrect()
	{
        if (registerPw == registerPw2)
            return true;
        else
            return false;
	}


    public void RegisterCorutine()
    {
        StartCoroutine(Register());
    }

    public void checkDuplicatedCorutine()
    {
        StartCoroutine(checkDuplicated());
    }

    public IEnumerator checkDuplicated()
	{
		WaitServer wait = new WaitServer();
        FirebaseRealtimeManager.Instance.readUser(registerId, wait);
        yield return wait.waitServer();
        User existUser = FirebaseRealtimeManager.Instance.user;
        if (existUser != null)
		{
            notDuplicatedId = false;
            Debug.Log("중복된 id가 있습니다.");
		}
        else
        {
            Debug.Log("중복된 id가 없습니다.");
        }
    }

    public IEnumerator Register()
    {
        User newUser = new User(registerId, registerPw, storeName, 0);
		WaitServer wait = new WaitServer();
        FirebaseRealtimeManager.Instance.readUser(newUser.id, wait);
        yield return wait.waitServer();
        User existUser = FirebaseRealtimeManager.Instance.user;
        if (existUser != null)
            Debug.Log("중복된 id가 있습니다.");
        else
        {
		    WaitServer wait2 = new WaitServer();
            FirebaseRealtimeManager.Instance.createUser(newUser.id, newUser, wait2);
            yield return wait2.waitServer();
            Debug.Log($"{newUser.id}가 등록되었습니다.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            if (GameObject.Find("Panel_SelectStore") != null)
                GameObject.Find("Panel_SelectStore").SetActive(false);
            else
                GameObject.Find("Canvas_PopRegister").transform.Find("Panel_PopCloseRegister").gameObject.SetActive(true);
		}
    }
}
