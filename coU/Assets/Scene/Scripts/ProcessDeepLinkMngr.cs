using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProcessDeepLinkMngr : MonoBehaviour
{
    public static ProcessDeepLinkMngr Instance { get; private set; }
    public string deeplinkURL;
    public static bool validScene = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Application.deepLinkActivated += onDeepLinkActivated;
            if (!String.IsNullOrEmpty(Application.absoluteURL))
            {
                // Cold start and Application.absoluteURL not null so process Deep Link.
                onDeepLinkActivated(Application.absoluteURL);
            }
            // Initialize DeepLink Manager global variable.
            else deeplinkURL = "[none]";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    class PrimaryKeys
    {
        public string scene { get; set; }
        public string name { get; set; }
        public string categoryMain { get; set; }
        public string categorySub { get; set; }

    }

    private void onDeepLinkActivated(string url) // 저절러 들어옴
    {
        // Update DeepLink Manager global variable, so URL can be accessed from anywhere.
        deeplinkURL = url;
        // Decode the URL to determine action. 
        // https://velog.io/@pear/Query-String-%EC%BF%BC%EB%A6%AC%EC%8A%A4%ED%8A%B8%EB%A7%81%EC%9D%B4%EB%9E%80
        // In this example, the app expects a link formatted like this:
        // unitydl://mylink?scene1
        // 42://coU?scene=scene1&name=""&categoryMain=""&categorySub=""
		
		string sceneName = "StoreScene";
		// 현재 방식
		// ("https://exgs.github.io/yunsleeMap/urlScheme.html?{0},{1},{2}",name,categoryMain,categorySub);
        string query = url.Split("?"[0])[1];
        PrimaryKeys pk = new PrimaryKeys();
        string[] parameters = query.Split(","[0]);
        if (parameters.Length != 3)
        {
            print("잘못된 URL Scheme 입니다." + "파라미터 갯수를 확인해주세요");
            // 이런 메세지가 toast로 나오도록 해야함.
            // toast에 마음대로 호출할 수 있는 함수를 만드는 것도 고려사항임
            Application.Quit();
            // 기기 별로 종료함수가 다른 것도 함수로 만들어놔야함.
        }
        else
        {
            pk.scene = sceneName;
            pk.name = parameters[1];
            pk.categoryMain = parameters[2];
            pk.categorySub = parameters[3];
        }

        if (validScene == true)
        {
            SceneManager.LoadScene(pk.scene);
            StoreSceneManager.storeName = pk.name;
			StoreSceneManager.categoryMain = pk.categoryMain;
            StoreSceneManager.categorySub = pk.categorySub;

            /*
             * StoreSceneManager에 있는 static 변수명
            public static string storeName = "사봉";
            public static string categorySub = "바디&향수";
            public static bool beforeScene = true;
            public static string searchStr = "";
            public static string floor = "";
             */

        }
    }
}