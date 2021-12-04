using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackBtnClick : MonoBehaviour
{
    public class SceneName
	{
        public const string AllCategoryScene = nameof(AllCategoryScene);
        public const string FirstScene = nameof(FirstScene);
        public const string LoadingScene = nameof(LoadingScene);
        public const string LoginScene = nameof(LoginScene);
        public const string MaxstScene = nameof(MaxstScene);
        public const string MenuScene = nameof(MenuScene);
        public const string RegisterScene = nameof(RegisterScene);
        public const string SearchScene = nameof(SearchScene);
        public const string StoreListScene = nameof(StoreListScene);
        public const string StoreScene = nameof(StoreScene);
        public const string UploadScene = nameof(UploadScene);
    }

    private int backCount = 0;
    // 12.4 Both Storescene and MaxstScene use this Function
    void ResetBackCount()
    {
        backCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.gameObject.scene.name)
		{
            case SceneName.AllCategoryScene:
                if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.sceneCount < 2)
				{
                    Stack.Instance.Clear();
                    SceneManager.LoadScene("MaxstScene");
                }
                break;
            case SceneName.FirstScene:
                break;
            case SceneName.LoadingScene:
                break;
            case SceneName.LoginScene:
                if (Input.GetKeyDown(KeyCode.Escape))
                    SceneManager.UnloadSceneAsync("LoginScene");
                break;
            case SceneName.MaxstScene:
                // 2021/10/18 hyojlee
                // MaxstScene에서 뒤로가기 연속 클릭 시 앱 종료하는 부분
                // 한 번 누르면 종료하지 않고 안드로이드의 토스트 메시지 뜨도록 함
                if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.sceneCount < 2)
                {
                    backCount++;
                    if (!IsInvoking("ResetBackCount"))
                        Invoke("ResetBackCount", 1.0f);
                    if (backCount == 2)
                    {
                        CancelInvoke("ResetBackCount");
                        Application.Quit();
                        #if !UNITY_EDITOR
                            System.Diagnostics.Process.GetCurrentProcess().Kill();
                        #endif
                    }
                        #if UNITY_EDITOR
                                            Debug.Log("한 번 더 누르시면 종료됩니다.");
                        #elif UNITY_ANDROID
			                                            Toast.ShowToastMessage("? ? ? ???? ?????.", Toast.Term.shortTerm);
                        #endif
                }
                break;
            case SceneName.MenuScene:
                if (Input.GetKeyDown(KeyCode.Escape))
                    SceneManager.UnloadSceneAsync("MenuScene");
                break;
            case SceneName.RegisterScene:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (GameObject.Find("Panel_SelectStore") != null)
                        GameObject.Find("Panel_SelectStore").SetActive(false);
                    else
                        GameObject.Find("Canvas_PopRegister").transform.Find("Panel_PopCloseRegister").gameObject.SetActive(true);
                }
                break;
            case SceneName.SearchScene:
                if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.sceneCount < 2)
                    BackBtnOnClick();
                void BackBtnOnClick()
                {
                    SceneInfo before = Stack.Instance.Pop();
                    string beforePath = SceneUtility.GetScenePathByBuildIndex(before.beforeScene);

                    if (beforePath.Contains("StoreScene"))
                    {
                        StoreSceneManager.storeName = before.storeName;
                        StoreSceneManager.categorySub = before.categorySub;
                    }
                    else if (beforePath.Contains("StoreListScene"))
                    {
                        DontDestroyManager.StoreList.categorySub = before.categorySub;
                    }
                    else //MaxstScene?? ??, AllCategoryScene?? ?? ?? ???? ?.
                        Stack.Instance.Clear();
                    SceneManager.LoadScene(before.beforeScene);
                }
                break;
            case SceneName.StoreListScene:
                if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.sceneCount < 2)
                    SceneManager.LoadScene("AllCategoryScene");
                break;
            case SceneName.StoreScene:
                if (Input.GetKeyDown(KeyCode.Escape) && (SceneManager.sceneCount == 1
                   || (SceneManager.sceneCount == 2 && SceneManager.GetActiveScene().name == "MaxstScene")))
                {
                    if (GameObject.Find("Panel_ChooseWhole") != null)
                    {
                        GameObject.Find("Panel_ChooseWhole").SetActive(false);
                    }
                    else if (Stack.Instance.Count() > 0)
                    {
                        BackBtnOnClick2();
                        void BackBtnOnClick2()
                        {
                            if (Stack.Instance.Count() == 0)
                            {
                                SceneManager.LoadScene("AllCategoryScene");
                                return;
                            }
                            SceneInfo before = Stack.Instance.Pop();
                            string beforePath = SceneUtility.GetScenePathByBuildIndex(before.beforeScene);
                            if (beforePath.Contains("MaxstScene"))
                            {
                                Stack.Instance.Clear();
                                SceneManager.UnloadSceneAsync("StoreScene");
                            }
                            else
                            {
                                if (beforePath.Contains("SearchScene"))
                                    DontDestroyManager.SearchScene.searchStr = before.storeName;
                                else if (beforePath.Contains("StoreListScene"))
                                    DontDestroyManager.StoreList.categorySub = before.categorySub;
                                else //MaxstScene?? ??, AllCategoryScene?? ?? ?? ???? ?.
                                    Stack.Instance.Clear();
                                SceneManager.LoadScene(before.beforeScene);
                            }
                        }
                    }
                    else
                    {
                        backCount++;
                        if (!IsInvoking("ResetBackCount"))
                            Invoke("ResetBackCount", 1.0f);
                        if (backCount == 2)
                        {
                            CancelInvoke("ResetBackCount");
                            Application.Quit();
                            #if !UNITY_EDITOR
	                            System.Diagnostics.Process.GetCurrentProcess().Kill();
                            #endif
                        }
                        #if UNITY_EDITOR
                            Debug.Log("한 번 더 누르시면 종료됩니다.");
                        #elif UNITY_ANDROID
                            Toast.ShowToastMessage("한 번 더 누르시면 종료됩니다.", Toast.Term.shortTerm);
                        #endif
                    }

                }
                break;
            case SceneName.UploadScene:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    GameObject.Find("Canvas_UploadPop").transform.Find("Panel_PopCloseUpload").gameObject.SetActive(true);
                }
                break;
            default:
                print($"\"{this.gameObject.scene.name}\"는 SceneName 클래스에 없습니다.");
                break;
        }
    }
}
