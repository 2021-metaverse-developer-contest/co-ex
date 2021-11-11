using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

[Table("Total")]
public class Total : MonoBehaviour
{
        // Stores
    [Column("index")]
    public int index { get; set; }
    [Column("name")]
    public string name { get; set; }
    [Column("floor")]
    public string floor { get; set; }
    [Column("phoneNumber")]
    public string phoneNumber { get; set; }
    [Column("openHour")]
    public string openHour { get; set; }
    [Column("categoryMain")]
    public string categoryMain { get; set; }
    [Column("categorySub")]
    public string categorySub { get; set; }
    [Column("logoPath")]
    public string logoPath { get; set; }
    [Column("tntSeq")]
    public string tntSeq { get; set; }
    [Column("x")]
    public double x { get; set; }
    [Column("y")]
    public double y { get; set; }
    [Column("modifiedX")]
    public double modifiedX { get; set; }
    [Column("modifiedy")]
    public double modifiedY { get; set; }
    [Column("mapKey")]
    public string mapKey { get; set; }

    // Catergory
    //[PrimaryKey]
    //[Column("name")]
    //public int name { get; set; }
    [Column("path")]
    public string path { get; set; }

    // Items
    //[Column("index")]
    //public int index { get; set; }
    //[Column("tntSeq")]
    //public string tntSeq { get; set; }
    [Column("itemTitle")]
    public string itemTitle { get; set; }
    [Column("itemTitleSub")]
    public string itemTitleSub { get; set; }
    [Column("itemPrice")]
    public int itemPrice { get; set; }
}
