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
using maxstAR;

public class putMarkerManager : MonoBehaviour
{
    public GameObject marker;
	private ARManager arManagr;
    [SerializeField]
    private float distanceRadius = 20.0f;
    private List<GameObject> canvasList;

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
        DBConnection(); // Start보다 윗단에 두어봄(찾지 못하는 문제 생기지 않도록)
    }

    void Start()
    {
        //DBConnection();
        StartCoroutine(activeStore());
    }

    public void DBConnection() //DB연결 상태 확인 코드
    {
        try
        {
            List<Store> stores = GetDBData.getStoresData("Select * from Stores S Where S.floor =\"B1\"");
            StartCoroutine(drawStore(stores, "B1"));
        }
        catch (Exception e)
        {
            Debug.Log(e);
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

    IEnumerator activeStore()
    {
        while (true)
        {
            List<GameObject> tempCanvasList = new List<GameObject>(canvasList);
            foreach (GameObject canvas in tempCanvasList)
            {
                if (isValidDistance(canvas.transform.position) == true)
                {
                    canvas.SetActive(true);
                }
                //yield return new WaitForSeconds(0.01f);
                yield return null;
            }
        }
    }



    IEnumerator drawStore(List<Store> stores, string floor)
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
            GameObject parent = GameObject.Find(floor);
            GameObject canvas = Instantiate(marker, parent.transform);
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
            canvas.transform.rotation = Quaternion.Euler(-arTransform.forward);
            canvas.name = it.name;
            #region 거리계산
            canvas.SetActive(false);
            
            #endregion

            yield return null;
        }
    }

    private Transform getARTransform()
    {
        GameObject arObject = arManagr.gameObject;
        Transform arTransform = arObject.transform;
        return (arTransform);
    }

    //IEnumerator drawStore(List<Store> stores, string floor)
    //{
    //    //int modifyX = -240;
    //    //int modifyY = 360;
    //    float yValue = 0f;
    //    if (floor == "B1")
    //        yValue = 2.5f;
    //    else if (floor == "B2")
    //        yValue = -5.0f;
    //    else if (floor == "1F")
    //        yValue = 2.5f;
    //    else
    //        yValue = 2.5f;

    //    foreach (var it in stores)
    //    {
    //        GameObject parent = GameObject.Find(floor);
    //        GameObject tempCircle = Instantiate(marker, parent.transform);

    //        Vector3 rawLocation = new Vector3(((float)it.modifiedX), ((float)it.modifiedY), 0);
    //        /* Legacy code
    //        //Vector3 rawLocation = new Vector3(
    //        //                            (LinearTransform.zeroPointAdjustionX + it.x / 100.0f + LinearTransform.marginX),
    //        //                            (LinearTransform.zeroPointAdjustionY - it.y / 100.0f + LinearTransform.marginY),
    //        //                            0);
    //        */

    //        tempCircle.transform.localPosition = rawLocation;
    //        tempCircle.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    //        tempCircle.name = it.name;
    //        yield return null;
    //    }
    //}
}
