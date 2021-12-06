using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackBtnClick : MonoBehaviour
{
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
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Stack.Instance.Clear();
                    GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
                    foreach (var obj in gameObjects)
                        if (obj.name == "Canvas_Parent")
                            obj.SetActive(true);
                    SceneManager.UnloadSceneAsync(this.gameObject.scene);
                }
                break;
            case SceneName.FirstScene:
                break;
            case SceneName.LoadingScene:
                break;
            case SceneName.LoginScene:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Additive);
                    SceneManager.UnloadSceneAsync("LoginScene");
                }
                break;
            case SceneName.MaxstScene:
                // 2021/10/18 hyojlee
                // MaxstScene에서 뒤로가기 연속 클릭 시 앱 종료하는 부분
                // 한 번 누르면 종료하지 않고 안드로이드의 토스트 메시지 뜨도록 함
                if (Input.GetKeyDown(KeyCode.Escape))
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
                break;
            case SceneName.MenuScene:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (Stack.Instance.Count() == 0)
                    {
                        GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
                        foreach (var obj in gameObjects)
                            if (obj.name == "Canvas_Parent")
                                obj.SetActive(true);
                    }
                    else
                    {
                        SceneInfo before = Stack.Instance.Pop();
                        string beforePath = SceneUtility.GetScenePathByBuildIndex(before.beforeScene);
                        if (beforePath.Contains("StoreScene"))
                        {
                            DontDestroyManager.StoreScene.storeName = before.storeName;
                            DontDestroyManager.StoreScene.categorySub = before.categorySub;
                        }
                        else if (beforePath.Contains("StoreListScene"))
                            DontDestroyManager.StoreListScene.categorySub = before.categorySub;
                        else if (beforePath.Contains("SearchScene"))
                            DontDestroyManager.SearchScene.searchStr = before.storeName;
                        else //MaxstScene으로 가던, AllCategoryScene으로 가던 스택 비워줘야 함.
                            Stack.Instance.Clear();
                        SceneManager.LoadSceneAsync(before.beforeScene, LoadSceneMode.Additive);
                    }
                    SceneManager.UnloadSceneAsync("MenuScene");
                }
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
                if (Input.GetKeyDown(KeyCode.Escape))
                    BackBtnOnClick();
                void BackBtnOnClick()
                {
                    if (Stack.Instance.Count() == 0)
                    {
                        GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
                        foreach (var obj in gameObjects)
                            if (obj.name == "Canvas_Parent")
                                obj.SetActive(true);
                    }
                    else
                    {
                        SceneInfo before = Stack.Instance.Pop();
                        string beforePath = SceneUtility.GetScenePathByBuildIndex(before.beforeScene);

                        if (beforePath.Contains("StoreScene"))
                        {
                            DontDestroyManager.StoreScene.storeName = before.storeName;
                            DontDestroyManager.StoreScene.categorySub = before.categorySub;
                        }
                        else if (beforePath.Contains("StoreListScene"))
                        {
                            DontDestroyManager.StoreListScene.categorySub = before.categorySub;
                        }
                        else //MaxstScene으로 가던, AllCategoryScene으로 가던 스택 비워줘야 함.
                            Stack.Instance.Clear();
                        SceneManager.LoadSceneAsync(before.beforeScene, LoadSceneMode.Additive);
                    }
                    SceneManager.UnloadSceneAsync("SearchScene");
                }
                break;
            case SceneName.StoreListScene:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Stack.Instance.Clear();
                    SceneManager.LoadSceneAsync("AllCategoryScene", LoadSceneMode.Additive);
                    SceneManager.UnloadSceneAsync("StoreListScene");
                }
                break;
            case SceneName.StoreScene:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (SceneManager.sceneCount == 1)
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
                    else
                    {
                        if (Stack.Instance.Count() == 0)
                        {
                            GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
                            foreach (var obj in gameObjects)
                                if (obj.name == "Canvas_Parent")
                                    obj.SetActive(true);
                        }
                        else
                        {
                            SceneInfo before = Stack.Instance.Pop();
                            string beforePath = SceneUtility.GetScenePathByBuildIndex(before.beforeScene);
                            if (beforePath.Contains("StoreScene"))
                            {
                                DontDestroyManager.StoreScene.storeName = before.storeName;
                                DontDestroyManager.StoreScene.categorySub = before.categorySub;
                            }
                            else if (beforePath.Contains("StoreListScene"))
                                DontDestroyManager.StoreListScene.categorySub = before.categorySub;
                            else if (beforePath.Contains("SearchScene"))
                                DontDestroyManager.SearchScene.searchStr = before.storeName;
                            else //MaxstScene으로 가던, AllCategoryScene으로 가던 스택 비워줘야 함.
                                Stack.Instance.Clear();
                            SceneManager.LoadSceneAsync(before.beforeScene, LoadSceneMode.Additive);
                        }
                        SceneManager.UnloadSceneAsync("StoreScene");
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
