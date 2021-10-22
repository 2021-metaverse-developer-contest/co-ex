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

public class putMarkerManager : MonoBehaviour
{
    public GameObject marker;
	private ARManager arManagr;
    private float initAlpha;

    [SerializeField]
    private float distanceRadius = 20.0f;
    public string floor = "B1"; // TODO: Trackable에서 인식하는 층수가 들어가야 함.
    private List<GameObject> canvasList = null;

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
        List<Store> stores = GetDBData.getStoresData("Select * from Stores S Where S.floor =\"" + floor + "\"");
        drawStore(stores, floor);
        initAlpha = canvasList[0].GetComponentInChildren<Image>().color.a;
        print("end awake()");
    }

    private void Update()
    {
        //print($"갯수: {canvasList.ToArray().Length}");
        //List<GameObject> tempCanvasList = canvasList;
        bool Panel_Background = true;
        foreach (GameObject canvas in canvasList)
        {

            if (Panel_Background == true)
            {
                if (GameObject.Find("Canvas_Overlay").transform.Find("Panel_Background").gameObject.active == false)
                    Panel_Background = false;
            }
            if (isValidDistance(canvas.transform.position) == true && Panel_Background == false)
            {
                canvas.SetActive(true);
                Transform arTransform = getARTransform();
				canvas.transform.forward = arTransform.forward;

                float distance = Vector3.Distance(arTransform.position, canvas.transform.position);
                if (distance > (distanceRadius / 2))
                {
                    Func<float,float,float> transAlpha= (alpha, distance) => alpha + ((0 - alpha) / (distanceRadius / 2)) * (distance - distanceRadius/2);
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
            }
            else
            {
                canvas.SetActive(false);
            }
        }
    }
    /*
     * 0 = 50, a = 25
     
     */

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
        if (floor == "B1")
            yValue = 2.5f;
        else if (floor == "B2")
            yValue = -5.0f;
        else if (floor == "1F")
            yValue = 2.5f;
        else
            yValue = 2.5f;


        foreach (var it in stores)
        {
            string parentOfStores = floor + "_Stores";
            GameObject parent = GameObject.Find(parentOfStores);
            GameObject canvas = Instantiate(marker, parent.transform);

            // hyojlee 2021/10/22
            // 미니 캔버스의 렌더 모드는 world space이므로 이벤트 카메라를 붙여줘야함.

            canvasList.Add(canvas);
            Transform infoParent = canvas.transform.Find("Panel_Whole").Find("Panel_StoreInfo");
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
                for (int i = 0; i < num; i++)
                {
                    menuName[i] = menuParent.Find("Panel_Name").Find("Panel_Name" + (i + 1).ToString()).GetComponentInChildren<TextMeshProUGUI>();
                    menuPrice[i] = menuParent.Find("Panel_Price").Find("Panel_Price" + (i + 1).ToString()).GetComponentInChildren<TextMeshProUGUI>();
                    menuName[i].text = items[i].itemTitle + "(" + items[i].itemTitleSub + ")";
                    menuPrice[i].text = string.Format("{0:n0}", items[i].itemPrice) + "원";
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
}
