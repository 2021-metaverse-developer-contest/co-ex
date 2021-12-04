using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using UnityEngine.Video;

public class StoreSceneManager : MonoBehaviour
{
    public static string storeName = "사봉";
    public static string categoryMain = "뷰티";
    public static string categorySub = "바디&향수";
    public static string floor = "";

    public GameObject[] imgs = new GameObject[5];
    public VideoPlayer[] videos = new VideoPlayer[3];
    List<StoreImg> storeImgList;

    List<Store> store;
    List<Item> item_List;
    GameObject Menu;
    int backCount = 0;

    public RectTransform content; //움직일 오브젝트
    private int count = 0; //나눠야할 값
    private float pos; //content의 LocalPosition
    private float movepos; //움직일 값
    private bool isScroll = false; //움직여야하는 지 구별
    private float imgWidth;
    float nextTime;
    float timeLeft = 5.0f;

    private void Awake()
	{
        storeImgList = new List<StoreImg>();
        List<Store> curStr = GetDBData.getStoresData($"Select * from Stores where name = '{storeName}';");
        categoryMain = curStr[0].categoryMain;
        categorySub = curStr[0].categorySub;
        floor = curStr[0].floor;
    }

	void Start()
    {
        Debug.Log($"Current Store {storeName}");
        nextTime = Time.time + timeLeft;
		imgWidth = imgs[0].GetComponent<RectTransform>().rect.width;
        Screen.orientation = ScreenOrientation.Portrait;

        if (Stack.Instance.Count() == 0)
            GameObject.Find("Btn_Back").SetActive(false);
        Scene s = SceneManager.GetSceneByName("StoreScene");
        GameObject[] gameObjects = s.GetRootGameObjects();
        print(gameObjects.Length);
        foreach (GameObject it in gameObjects)
        {
            print("This is GameObject" + it.name);
            if (it.name == "Canvas_Parent")
            {
                Menu = it.gameObject.transform.Find("Canvas_Main/Panel_Whole/Panel_Main/ScrollView_Main/Viewport/Content/Panel_MenuParent/Panel_Menu").gameObject;
                if (LoginSceneManager.IsPermission(storeName))
                    it.transform.Find("Canvas_Main/Panel_Whole/Panel_Main/ScrollView_Main/Viewport/Content/Panel_Info/Panel_Name/Btn_Upload").gameObject.SetActive(true);
            }
        }
        Debug.Log("StoreSceneManager start: StoreName " + storeName);
        Debug.Log("StoreSceneManager start: categorySub " + categorySub);

        GameObject.Find("Btn_Next").GetComponent<Button>().onClick.AddListener(delegate { Right(); });
        GameObject.Find("Btn_Prev").GetComponent<Button>().onClick.AddListener(delegate { Left(); });

        Debug.Log($"videos[0].isLooping = {videos[0].isLooping.ToString()}");
        Debug.Log($"videos[1].isLooping = {videos[0].isLooping.ToString()}");
        Debug.Log($"videos[2].isLooping = {videos[0].isLooping.ToString()}");
        videos[0].loopPointReached += Replay;
        videos[1].loopPointReached += Replay;
        videos[2].loopPointReached += Replay;
        InitialContent();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (SceneManager.sceneCount == 1
            || (SceneManager.sceneCount == 2 && SceneManager.GetActiveScene().name == "MaxstScene")))
        {
            if (GameObject.Find("Panel_ChooseWhole") != null)
            {
                GameObject.Find("Panel_ChooseWhole").SetActive(false);
            }
            else if (Stack.Instance.Count() > 0)
            {
                BackBtnOnClick();
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

        if (Time.time > nextTime)
        {
            nextTime = Time.time + timeLeft;
            Right();
        }
    }

    void ResetBackCount()
    {
        backCount = 0;
    }

    IEnumerator InitialImgs(Store store)
    {
        string logoPath = store.logoPath.Substring(0, store.logoPath.LastIndexOf("."));
		WaitServer wait = new WaitServer();
        FirebaseRealtimeManager firebaseRealtime = new FirebaseRealtimeManager();
        firebaseRealtime.readStoreImgs(storeName, wait);
        yield return wait.waitServer();
        //count = firebaseRealtime.ListStoreImgs.ToArray().Length;
        count = firebaseRealtime.ListStoreImgs.Count;
        int idx = count == 0 ? 1 : count;

        for ( ; idx < imgs.Length; idx++)
            imgs[idx].SetActive(false);
        if (count < 1)
        {
            Texture2D texture = Resources.Load(logoPath, typeof(Texture2D)) as Texture2D;
            if (texture == null)
                texture = Resources.Load("default_logo", typeof(Texture2D)) as Texture2D;
            imgs[0].GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
            Debug.Log($"storeName {store.name}");
        }
        else
		{
            movepos = imgWidth * (count - 1) / 2;
            while (Vector2.Distance(content.localPosition, new Vector2(movepos, 0)) >= 0.1f)
                content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
            pos = content.localPosition.x;
            firebaseRealtime.ListStoreImgs.Sort(StoreImg.sortOrdercmp);
            int i = 0;

            foreach (StoreImg img in firebaseRealtime.ListStoreImgs)
            {
		        WaitServer wait2 = new WaitServer();
                FirebaseStorageManager firebaseStorage = new FirebaseStorageManager();
                firebaseStorage.LoadFile(img, wait2);
                yield return wait2.waitServer();

                Uri uri = firebaseStorage.uri;
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                    Debug.Log($"UnityWebRequestError: {www.error}");
                else
                {
                    Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    imgs[i].GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                        new Vector2(.5f, .5f));
                    imgs[i].GetComponent<ImgClick>().storeName = store.name;
                    imgs[i++].GetComponent<ImgClick>().uri = uri;
                    Debug.Log($"{i - 1}번째 URI {uri.ToString()}");
                }
            }
		}
    }

    void InitialContent()
    {
        string query = "Select * from Stores Where name = '" + storeName + "'";
        query += " AND categorySub like '%" + categorySub + "%'"; //빈 문자열일 수도 있으니까
        Debug.Log("StoreSceneManager query = " + query);

        store = GetDBData.getStoresData(query);

        GameObject.Find("TMP_Name").GetComponent<TextMeshProUGUI>().text = store[0].name;
        GameObject.Find("TMP_CtMain").GetComponent<TextMeshProUGUI>().text = store[0].categoryMain + "/" + store[0].categorySub;
        GameObject.Find("TMP_Floor").GetComponent<TextMeshProUGUI>().text = store[0].floor;
        GameObject.Find("TMP_Phone").GetComponent<TextMeshProUGUI>().text = store[0].phoneNumber;
        GameObject.Find("TMP_Hour").GetComponent<TextMeshProUGUI>().text = store[0].openHour;
        storeName = store[0].name;
        categoryMain = store[0].categoryMain;
        categorySub = store[0].categorySub;
        floor = store[0].floor;

        InitialMenu(store[0].name);

        //이미지 띄우는 부분
        StartCoroutine(InitialImgs(store[0]));
		//      //Image img = GameObject.Find("Img_Logo").gameObject.GetComponent<Image>();
		//      Image img = imgs[0].GetComponent<Image>();
		//string path = store[0].logoPath;
		//      path = path.Substring(0, path.Length - 4);
		//      Texture2D texture = Resources.Load(path, typeof(Texture2D)) as Texture2D;
		//      if (texture == null)
		//          texture = Resources.Load("default_logo", typeof(Texture2D)) as Texture2D;
		//      img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);
		//      Debug.Log("name is " + store[0].name);

		//InitialMenu(store[0].name);
    }

    void InitialMenu(string sname)
    {
        string query = "select items.* from items " +
                " inner join stores " +
                " where stores.tntSeq = items.tntSeq " +
                " and stores.name = '" + store[0].name + "' " +
                " group by (itemTitle) " +
                " order by `index`";
        Debug.Log("StoreScene Menu query " + query);
        item_List = GetDBData.getItemsData(query);

        Debug.Log("StoreScene Number of Menu " + item_List.ToArray().Length.ToString());
        if (item_List.ToArray().Length < 1)
            Menu.SetActive(false);
        else
        {
            Menu.SetActive(true);
            GameObject MenuList= Menu.transform.Find("Panel_MenuList").gameObject;
            int maxCount = 3;
            int price;
            if (item_List.ToArray().Length < maxCount)
                maxCount = item_List.ToArray().Length;
            GameObject MenuNamePanel = MenuList.transform.Find("Panel_MenuName").gameObject;
            GameObject MenuPricePanel = MenuList.transform.Find("Panel_MenuPrice").gameObject;
            for (int i = 1; i <= maxCount; i++)
            {
                string menu = item_List[i - 1].itemTitle;
                if (item_List[i - 1].itemTitleSub != null)
                    menu += "(" + item_List[i - 1].itemTitleSub + ")";
                MenuNamePanel.transform.Find("TMP_MenuName" + i.ToString()).GetComponent<TextMeshProUGUI>().text = menu;
                price = item_List[i - 1].itemPrice;
                MenuPricePanel.transform.Find("TMP_MenuPrice" + i.ToString()).GetComponent<TextMeshProUGUI>().text = string.Format("{0:n0}", price) + "원";
            }
        }
    }

    void BackBtnOnClick()
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
                SearchSceneManager.searchStr = before.storeName;
            else if (beforePath.Contains("StoreListScene"))
                StoreListSceneManager.categorySub = before.categorySub;
            else //MaxstScene으로 가던, AllCategoryScene으로 가던 스택 비워줘야 함.
                Stack.Instance.Clear();
            SceneManager.LoadScene(before.beforeScene);
        }
    }

    public void Right()
    {
        //Debug.Log($"Start's right   movepos			  {movepos.ToString()}");
        //Debug.Log($"Start's right   content.rect.xMax {content.rect.xMax}");
        //Debug.Log($"Start's right   content.rect.xMin {content.rect.xMin}");
        //Debug.Log($"currentLocalPosition {content.localPosition.ToString()}");
        //Debug.Log($"Start's right {content.rect.xMin + content.rect.xMax / count}");
        if (count > 0)
        {
            if (Math.Round(content.rect.xMin + content.rect.xMax / count) == Math.Round(movepos))
            {
                movepos = imgWidth * (count - 1) / 2;
                while (Vector2.Distance(content.localPosition, new Vector2(movepos, 0)) >= 0.1f)
                    content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
                pos = content.localPosition.x;
                Debug.Log("Don't Move Right"); //Left로 가야됨
            }
            else
            {
                isScroll = true;
                movepos = pos - content.rect.width / count;
                pos = movepos;
                StartCoroutine(Scroll());
            }
        }
    }

    public void Left()
    {
        //Debug.Log($"Start's left   movepos			  {Math.Round(movepos).ToString()}");
        //Debug.Log($"Start's left   content.rect.xMax {content.rect.xMax}");
        //Debug.Log($"Start's left   content.rect.xMin {content.rect.xMin}");
        //Debug.Log($"currentLocalPosition {content.localPosition.ToString()}");
        //Debug.Log($"Start's left {content.rect.xMax - content.rect.xMax / count}");
        if (count > 0)
        {
            if (Math.Round(content.rect.xMax - content.rect.xMax / count) == Math.Round(movepos))
            {
                movepos = imgWidth * (count - 1) / 2 * -1;
                while (Vector2.Distance(content.localPosition, new Vector2(movepos, 0)) >= 0.1f)
                    content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
                pos = content.localPosition.x;
                Debug.Log("Don't Move Left"); //Right로 가야됨
            }
            else
            {
                isScroll = true;
                movepos = pos + content.rect.width / count;
                pos = movepos;
                StartCoroutine(Scroll());
            }
        }
    }

    IEnumerator Scroll()
    {
        nextTime = Time.time + timeLeft;
        while (isScroll)
        {
            content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
            if (Vector2.Distance(content.localPosition, new Vector2(movepos, 0)) < 0.1f)
            {
                Debug.Log($"Start's Coroutine {content.localPosition}");
                isScroll = false;
            }
            yield return null;
        }
    }

    void Replay(VideoPlayer vp)
    {
        vp.Play();
    }
}
