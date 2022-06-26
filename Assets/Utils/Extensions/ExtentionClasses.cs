using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Extensions
{
    public static class ExtenionClasses
    {
        public static void OnClickedByUser(this MonoBehaviour monoBehaviour)
        {
            Debug.Log("parent");
        }
    }
}