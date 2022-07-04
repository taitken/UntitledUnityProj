using System;
using UnityEngine;

namespace GameControllers.Models
{
    public class PathFinderMapItem : BaseModel
    {
        public PathFinderMapItem(int _x, int _y, bool _impassable = false) : base()
        {
            this.x = _x;
            this.y = _y;
            this.impassable = _impassable;
        }

        public static PathFinderMapItem Copy(PathFinderMapItem itemToCopy)
        {
            PathFinderMapItem newItem =new PathFinderMapItem(itemToCopy.x,itemToCopy.y);
            newItem.impassable = itemToCopy.impassable;
            newItem.preferToAvoid = itemToCopy.preferToAvoid;
            newItem.relativeSpeed = itemToCopy.relativeSpeed;
            newItem.distance = itemToCopy.distance;
            return newItem;
        }
        public int x { get; set; }
        public int y { get; set; }
        public bool impassable { get; set; }
        public bool preferToAvoid { get; set; }
        public float relativeSpeed { get; set; }
        public int? distance {get; set;}
    }
}

