using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreImg
{
    // 만약에 tranction이 가능하다면, 배열을 넣어두는 것도 좋은 방법이 될 수 있다.
    //public static List<string> sortOrderList = new List<string>();

    private string storeName;
    private string imgType;

    public int sortOrder { get; set; }
    public string imgPath;
    public DateTime dateTime {get; }
    public string imgName { get; }

    public StoreImg()
    {
    }

    // 막아 놓음
    //public StoreImg(string storeName, string imgPath, string imgType, int sortOrder, DateTime dateTime, string imgName)
    //{
    //    this.storeName = storeName;       계절밥상
    //    this.imgType = imgType;           jpg
    //    this.sortOrder = sortOrder;       1

    //    this.imgPath = imgPath;           계절밥상/DateTime_계절밥상.jpg
    //    this.dateTime = dateTime;         Date
    //    this.imgName = imgName;           DateTime_계절밥상.jpg
    //}


    public StoreImg(string storeName, string imgType, int sortOrder)
    {
        this.storeName = storeName;
        this.imgType = imgType;
        this.sortOrder = sortOrder;
        //
        this.dateTime = DateTime.Now;
        this.imgName = string.Format("{0}_{1}.{2}",
                        this.dateTime.ToString("MM/dd/yyyy HH:mm:ss").Replace("/", "_"),
                            storeName,
                                imgType);
        //
        string folderName = storeName;
        this.imgPath = Path.Combine(folderName, this.imgName);
    }

    public string getfullPath(string firebasestorageURL)
    {
        return Path.Combine(firebasestorageURL, imgPath);
    }
}
