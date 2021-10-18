using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AllCategorySceneManger : MonoBehaviour
{
    public GameObject MainItem;
    public GameObject SubItem;
    GameObject[] MainItems;
    GameObject[] SubItems;
    List<Category> MainItem_List;
    List<Store> SubItem_List;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        InitialCategoryMain();
        Debug.Log("AllCategorySceneManager start!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            backBtnClick();
    }

    void InitialCategorySub(string categoryMain)
    {
        string query = "Select distinct categorySub from Stores where categoryMain = '" + categoryMain + "'";
        SubItem_List = GetDBData.getStoresData(query);
        SubItem_List.Distinct().ToList();

        SubItems = new GameObject[SubItem_List.ToArray().Length];
    }

    void InitialCategoryMain()
    {
        string query = "Select distinct categoryMain as name, path from Stores, Category where Stores.categoryMain = Category.name";
        MainItem_List = GetDBData.getCategoryData(query);

        MainItems = new GameObject[MainItem_List.ToArray().Length];
        for (int i = 0; i < MainItems.Length; i++)
        {
            MainItems[i] = Instantiate(MainItem, GameObject.Find("Content").transform);
            MainItems[i].GetComponentInChildren<TextMeshProUGUI>().text = MainItem_List[i].name;
            Texture2D texture = Resources.Load(MainItem_List[i].path, typeof(Texture)) as Texture2D;
            if (texture != null)
                MainItems[i].transform.Find("Panel_MainCt").transform.Find("Panel_Left").GetComponentInChildren<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);

            InitialCategorySub(MainItem_List[i].name);
            for (int j = 0; j < SubItems.Length; j++)
            {
                SubItems[j] = Instantiate(SubItem, MainItems[i].transform.Find("Panel_SubCt").transform);
                SubItems[j].GetComponentInChildren<TextMeshProUGUI>().text = SubItem_List[j].categorySub;
            }
        }
    }

    void backBtnClick()
    {
        SceneManager.LoadScene("MaxstScene");
    }
}
