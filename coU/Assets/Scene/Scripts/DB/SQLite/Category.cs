using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

[Table("Categories")]
public class Category
{
    [Column("name")]
    [PrimaryKey]
    public string name { get; set; }
    [Column("path")]
    public string path { get; set; }
}
