using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;

public class ProcessDeepLinkMngr : MonoBehaviour
{
    public static ProcessDeepLinkMngr Instance { get; private set; }
    public string deeplinkURL;
    public static bool validScene = true;

    private void Awake()
    {
        print("Awake함수 호출");
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
            else deeplinkURL = "maxst://vpssdk?아쿠아리움,엔터테인먼트,아쿠아리움";
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
        print("onDeepLinkActivated 호출");
        // Update DeepLink Manager global variable, so URL can be accessed from anywhere.

        // 안정장치를 해야하나?
        //Awake();

        // Decode the URL to determine action. 
        print(url);
        deeplinkURL = WebUtility.UrlDecode(url);
        print(deeplinkURL);

        string sceneName = "StoreScene";
        // 현재 url이 들어오는 방식: maxst://vpssdk?아쿠아리움,엔터테인먼트,아쿠아리움"
        string query = deeplinkURL.Split("?"[0])[1];
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
            pk.name = parameters[0];
            pk.categoryMain = parameters[1];
            pk.categorySub = parameters[2];
        }

        if (validScene == true)
        {
            SceneManager.LoadScene(pk.scene);
            DontDestroyManager.StoreScene.storeName = pk.name;
			DontDestroyManager.StoreScene.categoryMain = pk.categoryMain;
            DontDestroyManager.StoreScene.categorySub = pk.categorySub;
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