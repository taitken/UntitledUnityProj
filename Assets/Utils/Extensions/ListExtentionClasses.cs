using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace System.Collections.Generic
{
    public static class ListExtenionClasses
    {
        public static t1 Find<t1>(this IList<t1> list, Func<t1, bool> callback)
        {
            foreach (t1 listItem in list)
            {
                if (callback(listItem))
                {
                    return listItem;
                }
            }
            return default(t1);
        }

        public static bool Any<t1>(this IList<t1> list, Func<t1, bool> callback)
        {
            foreach (t1 listItem in list)
            {
                if (callback(listItem))
                {
                    return true;
                }
            }
            return false;
        }

        public static IList<t1> Filter<t1>(this IList<t1> list, Func<t1, bool> callback)
        {
            IList<t1> returnList = new List<t1>();
            foreach (t1 listItem in list)
            {
                if (callback(listItem))
                {
                    returnList.Add(listItem);
                }
            }
            return returnList;
        }

        public static IList<t2> Map<t1, t2>(this IList<t1> list, Func<t1, t2> callback)
        {
            IList<t2> returnList = new List<t2>();
            foreach (t1 listItem in list)
            {
                returnList.Add(callback(listItem));
            }
            return returnList;
        }

        public static void ForEach<t1>(this IList<t1> list, Action<t1> callback)
        {
            foreach (t1 listItem in list)
            {
                callback(listItem);
            }
        }

        public static void ForEach<t1>(this IList<t1> list, Action<t1, int> callback)
        {
            for (int i = 0; i < list.Count; i++)
            {
                callback(list[i], i);
            }
        }

        public static Vector3Int ConvertToVector3Int(this Vector3 vec3)
        {
            return new Vector3Int((int)vec3.x, (int)vec3.y, (int)vec3.z);
        }

        public static void ForEach(this Vector3Int vector3, Action<int, int> callback)
        {
            for (int x = 0; x < vector3.x; x++)
            {
                for (int y = 0; y < vector3.y; y++)
                {
                    callback(x, y);
                }
            }
        }

        public static void ForEach(this Vector3Int vector3, Action<int, int, int> callback)
        {
            for (int x = 0; x < vector3.x; x++)
            {
                for (int y = 0; y < vector3.y; y++)
                {
                    for (int z = 0; z < vector3.z; z++)
                    {
                        callback(x, y, z);

                    }
                }
            }
        }

        public static void ForEach(this Tilemap tilemap, Action<Tile> callback)
        {
            tilemap.size.ForEach((x, y, z) =>
            {
                callback(tilemap.GetTile<Tile>(new Vector3Int(x, y, z)));
            });
        }

        // Compares this list to the parameter list, and will match items by ID. 
        // Returns those that are in this list but not the parameter list.
        public static IList<t> GetNewModels<t>(this IList<t> newList, IList<t> oldList) where t : BaseModel
        {
            return newList.Filter(newModel => { return oldList.Find(oldModel => { return oldModel.ID == newModel.ID; }) == null; });
        }

        // Compares this list to the parameter list, and will match items by ID. 
        // Returns those that are in the parameter list, but not this list.
        public static IList<t> GetRemovedModels<t>(this IList<t> newList, IList<t> oldList) where t : BaseModel
        {
            return oldList.Filter(oldModel => { return newList.Find(newModel => { return newModel.ID == oldModel.ID; }) == null; });
        }
    }
}