using System.Globalization;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace GameControllers.Models
{
    public class PathFinderMap : BaseModel
    {
        public PathFinderMapItem[,] mapitems;

        public PathFinderMap(PathFinderMapItem[,] newMap)
        {
            this.mapitems = newMap;
        }

        public static PathFinderMap Copy(PathFinderMap itemToCopy)
        {
            PathFinderMapItem[,] newMap = new PathFinderMapItem[itemToCopy.mapitems.GetLength(0), itemToCopy.mapitems.GetLength(1)];
            for (int x = 0; x < itemToCopy.mapitems.GetLength(0); x++)
            {
                for (int y = 0; y < itemToCopy.mapitems.GetLength(1); y++)
                {
                    newMap[x, y] = PathFinderMapItem.Copy(itemToCopy.mapitems[x, y]);
                }
            }
            return new PathFinderMap(newMap);
        }
        public int height { get; private set; }
        public int width { get; private set; }

        public PathFinderMapItem GetMapItemAt(int x, int y)
        {
            return (x < mapitems.GetLength(0) && x >= 0 && y >= 0 && y < mapitems.GetLength(1)) ? mapitems[x, y] : null;
        }

        public PathFinderMapItem GetPassableMapItemAt(int x, int y)
        {
            PathFinderMapItem item = this.GetMapItemAt(x, y);
            return item == null || item.impassable ? null : item;
        }

        public void Refresh()
        {
            for (int x = 0; x < this.mapitems.GetLength(0); x++)
            {
                for (int y = 0; y < this.mapitems.GetLength(1); y++)
                {
                    this.mapitems[x, y].distance = null;
                }
            }
        }
    }
}

