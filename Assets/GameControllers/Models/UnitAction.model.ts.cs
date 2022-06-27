using System;
using System.Collections.Generic;
using UnityEngine;

public enum eActionCategories
{
    Dig,
    Build
}

public class UnitActionModel : BaseModel
{
    public long ID { get; set; }
    public eActionCategories actionCategory { get; set; }
    public string actionName { get; set; }
    public float priority { get; set; }

}
