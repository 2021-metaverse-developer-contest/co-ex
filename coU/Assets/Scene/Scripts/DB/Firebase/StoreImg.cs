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
    //public StoreImg(string storeName, string imgPath, string imgType, long sortOrder, string dateTime, string imgName)
    //{
    //    this.storeName = storeName;       계절밥상
    //    this.imgType = imgType;           jpg
    //    this.sortOrder = sortOrder;       1

    //    this.imgPath = imgPath;           계절밥상/DateTime_계절밥상.jpg
    //    this.dateTime = dateTime;         Date
    //    this.imgName = imgName;           DateTime_계절밥상.jpg
    //}


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

    public string getfullPath(string firebasestorageURL)
    {
        return Path.Combine(firebasestorageURL, imgPath);
    }
}
