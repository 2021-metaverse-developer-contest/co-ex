using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreImg
{
    public string storeName;
    public string imgType;
    public long sortOrder;
    public string imgPath;
    public string dateTime;
    public string imgName;

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

	public StoreImg(string storeName, string imgType, long sortOrder, string imgPath, string dateTime, string imgName)
	{
		this.storeName = storeName;
		this.imgType = imgType;
		this.sortOrder = sortOrder;
		this.imgPath = imgPath;
		this.dateTime = dateTime;
		this.imgName = imgName;

	}

	public StoreImg(ref StoreImg other)
    {
        this.imgPath = other.imgPath;
        this.storeName = other.storeName;
        this.imgType = other.imgType;
        this.sortOrder = other.sortOrder;

        this.dateTime = other.dateTime;
        this.imgName = other.imgName;
    }

    public StoreImg(string imgPath)
    {
        this.imgPath = imgPath;
        this.storeName = imgPath.Split('/')[0];
        this.imgType = imgPath.Substring(imgPath.LastIndexOf('.') + 1);
        this.sortOrder = 0;

        this.dateTime = imgPath.Split('/')[1].Split('_')[0];
        this.imgName = imgPath.Split('/')[1];
    }

    public StoreImg(string imgPath, long sortOrder)
    {
        this.imgPath = imgPath;
        this.storeName = imgPath.Split('/')[0];
        this.imgType = imgPath.Substring(imgPath.LastIndexOf('.') + 1);
        this.sortOrder = sortOrder;

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
        this.imgPath = Path.Combine(storeName.Replace('.', '_'), this.imgName);
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
        string[] imgPaths = imgPath.Split('/');
        imgPaths[0] = imgPaths[0].Replace('.', '_');
        return Path.Combine(firebasestorageURL, imgPaths[0], imgPaths[1]);
    }

    public static void swapSortOrder(StoreImg a, StoreImg b) // 둘 사이 sortOrder 교환
    {
        long temp = a.sortOrder;
        a.sortOrder = b.sortOrder;
        b.sortOrder = temp;
    }

    public static int sortOrdercmp(StoreImg a, StoreImg b) // sortOrder에 따라 오름차순 정렬
    {
        if (a.sortOrder < b.sortOrder)
            return (-1);
        else if (a.sortOrder == b.sortOrder)
            return (0);
        else
            return (1);
    }
}
