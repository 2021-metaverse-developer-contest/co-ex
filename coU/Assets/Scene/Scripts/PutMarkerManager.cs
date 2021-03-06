using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using SQLite4Unity3d;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using maxstAR;

public class PutMarkerManager : MonoBehaviour
{
    public GameObject marker;
	private ARManager arManagr;
    private float initAlpha;

    [SerializeField]
    private float distanceRadius = 20.0f;
    public static string floor = "B1"; // TODO: Trackable에서 인식하는 층수가 들어가야 함.
    private List<GameObject> canvasList = new List<GameObject>();

    class LinearTransform
    {
        public const float marginX = -2.40f;
        public const float zeroPointAdjustionX = -35f;
        public const float marginY = 3.60f;
        public const float zeroPointAdjustionY = 17.19f;
    }

    void Awake()
    {
        arManagr = FindObjectOfType<ARManager>();
        canvasList = new List<GameObject>();
        floor = floor.Replace("landmark_coex_", "");
    }

    void timetoDraw()
    {
        if (floor != "OUTDOOR")
        {
            string query = "Select * from Stores Where floor = '" + floor + "' group by tntSeq order by `index`";
            List<Store> stores = GetDBData.getStoresData(query);
            drawStore(stores, floor);
            initAlpha = canvasList[0].GetComponentInChildren<Image>().color.a;
        }
    }

    void drawTransparent()
    {
        foreach (GameObject canvas in canvasList)
        {
            if (canvas == null)
                break;
            if (isValidDistance(canvas.transform.position) == true)
            {
                canvas.SetActive(true);
                canvas.GetComponent<ImgScrollingMini>().inRadiusRange = true;
                Transform arTransform = getARTransform();
                canvas.transform.forward = arTransform.forward;

                float distance = Vector3.Distance(arTransform.position, canvas.transform.position);
                if (distance > (distanceRadius / 2))
                {
                    Func<float, float, float> transAlpha = (alpha, distance) => alpha + ((0 - alpha) / (distanceRadius / 2)) * (distance - distanceRadius / 2);
                    Func<Color, float, float, Color> transColor = (src, alpha, distance) =>
                    {
                        Color tempColor = src;
                        tempColor.a = transAlpha(alpha, distance);
                        src = tempColor;
                        return (tempColor);
                    };

                    Image[] images = canvas.GetComponentsInChildren<Image>();
                    TextMeshProUGUI[] textmeshes = canvas.GetComponentsInChildren<TextMeshProUGUI>();
                    foreach (Image it in images)
                    {
                        it.color = transColor(it.color, initAlpha, distance);
                    }
                    foreach (TextMeshProUGUI it in textmeshes)
                    {
                        it.color = transColor(it.color, initAlpha, distance);
                    }
                }
                Color bellColor = canvas.transform.Find("Panel_Whole/Panel_StoreInfoParent/Img_bell").GetComponent<Image>().color;
                if (bellColor.a > 0.5)
                    bellColor.a = 1;
                canvas.transform.Find("Panel_Whole/Panel_StoreInfoParent/Img_bell").GetComponent<Image>().color = bellColor;
            }
            else
            {
                canvas.SetActive(false);
                // canvas.GetComponent<ImgScrollingMini>().inRadiusRange = false;
                /// 이미 true로 storeImgs가 로드 됬으면(true), 이미지가 언로드(false) 될 필요가 없어서, ImgScrollingMini에서 false 일 때를 따로 구현하지 않았음
            }
        }
    }

    private bool isfinishDetect = false;

    private void Update()
    {
        //print($"갯수: {canvasList.ToArray().Length}");
        //List<GameObject> tempCanvasList = canvasList;
        if (isfinishDetect == false)
        {
            if (GameObject.Find("Canvas_Overlay").transform.Find("Panel_Background").gameObject.active == false)
			{
                timetoDraw();
                isfinishDetect = true;
			}
        }
        else
		{
            drawTransparent();
		}
    }

    private bool isValidDistance(Vector3 storePosition)
    {
        Transform arTransform = getARTransform();
        if (Vector3.Distance(arTransform.position, storePosition) < distanceRadius)
            return (true);
        else
            return (false);
    }

    void drawStore(List<Store> stores, string floor)
    {
        //int modifyX = -240;
        //int modifyY = 360;
        float yValue = 0f;
        if (floor == "B2")
            yValue = -1.0f;
        else if (floor == "1F")
            yValue = 7.0f;
        else
            yValue = 4.5f;

        foreach (var it in stores)
        {
            string parentOfStores = floor + "_Stores";
            GameObject parent = GameObject.Find(parentOfStores);
            GameObject canvas = Instantiate(marker, parent.transform);
            canvas.tag = "minicanvas";

            // hyojlee 2021/10/22
            // 미니 캔버스의 렌더 모드는 world space이므로 이벤트 카메라를 붙여줘야함.

            canvasList.Add(canvas);
            Transform infoParent = canvas.transform.Find("Panel_Whole").Find("Panel_StoreInfoParent/Panel_StoreInfo");
            Transform menuParent = canvas.transform.Find("Panel_Whole").Find("Panel_StoreMenu").Find("Panel_Menu");

            TextMeshProUGUI name = infoParent.Find("TMP_StoreName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI open = infoParent.Find("TMP_StoreOpen").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI[] menuName = new TextMeshProUGUI[3];
            TextMeshProUGUI[] menuPrice = new TextMeshProUGUI[3];

            name.text = it.name;
            open.text = it.openHour;
            List<Item> items = GetDBData.getItemsData("Select * from items where tntSeq = '" + it.tntSeq + "'");
            if (items.ToArray().Length != 0)
            {
                menuParent.parent.gameObject.SetActive(true);
                int num = menuName.Length < items.ToArray().Length ? menuName.Length : items.ToArray().Length;
                for (int i = 0; i < menuName.Length; i++)
                {
                    if (i < num)
                    {
                        menuName[i] = menuParent.Find("Panel_Name").Find("Panel_Name" + (i + 1).ToString()).GetComponentInChildren<TextMeshProUGUI>();
                        menuPrice[i] = menuParent.Find("Panel_Price").Find("Panel_Price" + (i + 1).ToString()).GetComponentInChildren<TextMeshProUGUI>();
                        menuName[i].text = items[i].itemTitle;
                        if (items[i].itemTitleSub != "" && items[i].itemTitleSub != null)
                            menuName[i].text += "(" + items[i].itemTitleSub + ")";
                        menuPrice[i].text = string.Format("{0:n0}", items[i].itemPrice) + "원";
                    }
                    else
                    {
                        menuParent.Find("Panel_Name").Find("Panel_Name" + (i + 1).ToString()).gameObject.SetActive(false);
                        menuParent.Find("Panel_Price").Find("Panel_Price" + (i + 1).ToString()).gameObject.SetActive(false);
                    }
                }
            }
            else
                menuParent.parent.gameObject.SetActive(false);

            Vector3 rawLocation = new Vector3(((float)it.modifiedX), ((float)it.modifiedY), 0);
            /* Legacy code
            //Vector3 rawLocation = new Vector3(
            //                            (LinearTransform.zeroPointAdjustionX + it.x / 100.0f + LinearTransform.marginX),
            //                            (LinearTransform.zeroPointAdjustionY - it.y / 100.0f + LinearTransform.marginY),
            //                            0);
            */

            
            Transform arTransform = getARTransform();
            canvas.transform.localPosition = rawLocation;
            Vector3 temp = new Vector3(canvas.transform.position.x, yValue, canvas.transform.position.z);
            canvas.transform.position = temp;
            canvas.transform.forward = arTransform.forward;
            canvas.name = it.name;
            canvas.SetActive(false);
        }
    }

    private Transform getARTransform()
    {
        GameObject arObject = arManagr.gameObject;
        Transform arTransform = arObject.transform;
        return (arTransform);
    }

    // 혹시 몰라서 멤버변수 canvasList를 날림
	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
		}
		else
		{
            isfinishDetect = false;
            if (canvasList.Count != 0)
            {
                foreach (var i in canvasList)
                {
                    Destroy(i);
                }
            }
            canvasList.Clear();
		}
	}
}
