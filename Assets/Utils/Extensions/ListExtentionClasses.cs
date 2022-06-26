using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace System.Collections.Generic
{
    public static class ListExtenionClasses
    {
        public static t1 Find<t1>(this IList<t1> list, Func<t1,bool> callback)
        {
            foreach(t1 listItem in list)
            {
                if(callback(listItem))
                {
                    return listItem;
                }
            }
            return default(t1);
        }
        public static void OnClickedByUser(this MonoBehaviour monoBehaviour)
        {
            Debug.Log("parent");
        }
    }
}