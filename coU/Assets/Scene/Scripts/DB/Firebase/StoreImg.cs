using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreImg
{
    public string storeName { get; set; }
    public string imgType { get; set; }
    public long sortOrder { get; set; }
    public string imgPath { get; set; }
    public string dateTime { get; set; }
    public string imgName { get; set; }

    public StoreImg()
    {
    }

    // 막아 놓음
    //public StoreImg(string storeName, string imgType, long sortOrder, string imgPath, string dateTime, string imgName)
    //{
    //    this.storeName = storeName;       계절밥상
    //    this.imgType = imgType;           jpg
    //    this.sortOrder = sortOrder;       1

    //    this.imgPath = imgPath;           계절밥상/DateTime_계절밥상.jpg
    //    this.dateTime = dateTime;         Date
    //    this.imgName = imgName;           DateTime_계절밥상.jpg
    //}

    public StoreImg(string imgPath)
    {
        this.imgPath = imgPath;
        this.storeName = imgPath.Split('/')[0];
        this.imgType = imgPath.Substring(imgPath.LastIndexOf('.') + 1);
        this.sortOrder = 0;

        this.dateTime = imgPath.Split('/')[1].Split('_')[0];
        this.imgName = imgPath.Split('/')[1];
    }

    public StoreImg(string storeName, string imgType, long sortOrder)
    {
        this.storeName = storeName;
        this.imgType = imgType;
        this.sortOrder = sortOrder;
        //
        this.dateTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        this.imgName = string.Format("{0}_{1}.{2}", this.dateTime, storeName, imgType);
        //
        this.imgPath = Path.Combine(storeName, this.imgName);
    }

    public void printAllValues()
	{
        Debug.Log($"storeName: {storeName}");
        Debug.Log($"imgType: {imgType}");
        Debug.Log($"sortOrder: {sortOrder}");
        Debug.Log($"imgPath: {imgPath}");
        Debug.Log($"dateTime: {dateTime}");
        Debug.Log($"imgName: {imgName}");
    }

    public static void printAllValues(string storeName, string imgType, long sortOrder, string imgPath, string dateTime, string imgName)
    {
        Debug.Log($"storeName: {storeName}");
        Debug.Log($"imgType: {imgType}");
        Debug.Log($"sortOrder: {sortOrder}");
        Debug.Log($"imgPath: {imgPath}");
        Debug.Log($"dateTime: {dateTime}");
        Debug.Log($"imgName: {imgName}");
    }

    public string getfullPath(string firebasestorageURL)
    {
        return Path.Combine(firebasestorageURL, imgPath);
    }
}
