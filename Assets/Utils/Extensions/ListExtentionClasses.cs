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

        public static void ForEach<t1>(this IList<t1> list, Action<t1> callback)
        {
            foreach (t1 listItem in list)
            {
                callback(listItem);
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
    }
}