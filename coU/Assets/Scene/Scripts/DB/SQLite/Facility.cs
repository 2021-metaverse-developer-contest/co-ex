using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

[Table("Facilities")]
public class Facility : MonoBehaviour
{
    [Column("floor")]
    public string floor { get; set; }
    [Column("type")]
    public string type { get; set; }
    [Column("modifiedX")]
    public double modifiedX { get; set; }
    [Column("modifiedY")]
    public double modifiedY { get; set; }
}