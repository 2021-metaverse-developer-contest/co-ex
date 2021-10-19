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

public class putMarkerManager : MonoBehaviour
{
    public GameObject marker;


    class LinearTransform
    {
        public const float marginX = -2.40f;
        public const float zeroPointAdjustionX = -35f;
        public const float marginY = 3.60f;
        public const float zeroPointAdjustionY = 17.19f;
    }

    void Start()
    {
        DBConnection();
    }

    void Update()
    {

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

            canvas.transform.localPosition = rawLocation;
            canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            canvas.name = it.name;
            yield return null;
        }
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
