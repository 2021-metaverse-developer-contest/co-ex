using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using SQLite4Unity3d;
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
            List<Stores> stores = getDBData.getStoresData("Select * from Stores S Where S.floor =\"B1\"");
            StartCoroutine(drawStore(stores, "B1"));
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }



    IEnumerator drawStore(List<Stores> stores, string floor)
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
            GameObject tempCircle = Instantiate(marker, parent.transform);

            Vector3 rawLocation = new Vector3(((float)it.modifiedX), ((float)it.modifiedY), 0);
            /* Legacy code
            //Vector3 rawLocation = new Vector3(
            //                            (LinearTransform.zeroPointAdjustionX + it.x / 100.0f + LinearTransform.marginX),
            //                            (LinearTransform.zeroPointAdjustionY - it.y / 100.0f + LinearTransform.marginY),
            //                            0);
            */

            tempCircle.transform.localPosition = rawLocation;
            tempCircle.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            tempCircle.name = it.name;
            yield return null;
        }
    }
}
