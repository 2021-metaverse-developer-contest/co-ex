using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

[Table("Items")]
public class Items : MonoBehaviour
{
    [Column("index")]
    public int index { get; set; }
    [Column("tntSeq")]
    public string tntSeq { get; set; }
    [Column("itemTitle")]
    public string itemTitle { get; set; }
    [Column("itemTitleSub")]
    public string itemTitleSub { get; set; }
    [Column("itemPrice")]
    public int itemPrice { get; set; }
}
