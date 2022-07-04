using System;
using UnityEngine;
using System.Collections.Generic;

namespace GameControllers.Models
{
    public class PathFinderMap : BaseModel
    {
        private IList<IList<PathFinderMapItem>> _mapitems;
        public IList<IList<PathFinderMapItem>> mapitems
        {
            get { return _mapitems; }
            set
            {
                if (value.Find(yVals => { return yVals.Count != value[0].Count; }) != null)
                {
                    throw new Exception(
                        $"{nameof(value)} must have all inner lists of equal length.");
                }
                _mapitems = value;
                height = mapitems.Count > 0 ? mapitems[0].Count : 0;
                width = mapitems.Count;
            }
        }

        public PathFinderMap(IList<IList<PathFinderMapItem>> newMap)
        {
            this.mapitems = newMap;
        }

        public static PathFinderMap Copy(PathFinderMap itemToCopy)
        {
            PathFinderMap newMap = new PathFinderMap(itemToCopy.mapitems.Map(column => { return column.Map(item => { return PathFinderMapItem.Copy(item); }); }));
            return newMap;
        }
        public int height { get; private set; }
        public int width { get; private set; }

        public PathFinderMapItem GetMapItemAt(int x, int y)
        {
            return (x < mapitems.Count && x >= 0 && y >= 0 && y < mapitems[0].Count) ? mapitems[x][y] : null;
        }

        public PathFinderMapItem GetPassableMapItemAt(int x, int y)
        {
            PathFinderMapItem item = this.GetMapItemAt(x,y);
            return item == null || item.impassable ? null : item;
        }

        public void Refresh()
        {
            this.mapitems.ForEach(column =>
            {
                column.ForEach(item =>
                {
                    item.distance = null;
                });
            });
        }
    }
}

