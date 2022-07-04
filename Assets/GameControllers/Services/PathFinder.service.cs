using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;

namespace GameControllers.Services
{
    public class PathFinderService : IPathFinderService
    {
        public Subscribable<PathFinderMap> pathFinderMap { get; set; } = new Subscribable<PathFinderMap>(new PathFinderMap(new List<IList<PathFinderMapItem>>()));

        public IList<Vector3Int> FindPath(Vector3Int startingPos, Vector3Int endPos, PathFinderMap _map)
        {
            if (_map == null) return null;
            bool pathFound = false;
            PathFinderMapItem endPosItem = _map.GetMapItemAt(endPos.x, endPos.y);
            if (endPosItem == null) return null;
            endPosItem.distance = 0;
            List<PathFinderMapItem> neighbours = new List<PathFinderMapItem> { endPosItem };

            while (neighbours.Count > 0)
            {
                List<PathFinderMapItem> newNeighbours = IncrementNeigbours(neighbours[0], _map);
                if (newNeighbours.Find(item => { return item.x == startingPos.x && item.y == startingPos.y; }) != null)
                {
                    pathFound = true;
                    break;
                }
                neighbours.AddRange(newNeighbours);
                neighbours.RemoveAt(0);
            }
            IList<Vector3Int> returnMap = pathFound ? PathBack(startingPos, _map) : null;
            this.pathFinderMap.Get().Refresh();
            return returnMap;
        }

        private List<PathFinderMapItem> IncrementNeigbours(PathFinderMapItem item, PathFinderMap _map)
        {
            List<PathFinderMapItem> neighbours = new List<PathFinderMapItem>();
            neighbours.Add(_map.GetPassableMapItemAt(item.x, item.y - 1));
            neighbours.Add(_map.GetPassableMapItemAt(item.x - 1, item.y));
            neighbours.Add(_map.GetPassableMapItemAt(item.x + 1, item.y));
            neighbours.Add(_map.GetPassableMapItemAt(item.x, item.y + 1));
            neighbours.Add(_map.GetPassableMapItemAt(item.x - 1, item.y - 1));
            neighbours.Add(_map.GetPassableMapItemAt(item.x + 1, item.y - 1));
            neighbours.Add(_map.GetPassableMapItemAt(item.x - 1, item.y + 1));
            neighbours.Add(_map.GetPassableMapItemAt(item.x + 1, item.y + 1));
            // Remove out of bounds (null) tiles, and items that have already been assigned a lower
            neighbours = (List<PathFinderMapItem>)neighbours.Filter(neighbour => { return neighbour != null && !(neighbour.distance != null && neighbour.distance < item.distance); });
            neighbours.ForEach(neighbour => { neighbour.distance = item.distance + 1; });
            return neighbours;
        }

        private IList<Vector3Int> PathBack(Vector3Int startingPos, PathFinderMap _map)
        {
            IList<Vector3Int> pathBack = new List<Vector3Int> { startingPos };
            PathFinderMapItem startingItem = _map.GetMapItemAt(startingPos.x, startingPos.y);
            for (int i = (int)startingItem.distance - 1; i >= 0; i--)
            {
                Vector3Int nextItem = pathBack[pathBack.Count - 1];
                if (_map.GetMapItemAt(nextItem.x, nextItem.y - 1).distance == i)
                {
                    pathBack.Add(new Vector3Int(nextItem.x, nextItem.y - 1));
                    continue;
                }
                if (_map.GetMapItemAt(nextItem.x - 1, nextItem.y).distance == i)
                {
                    pathBack.Add(new Vector3Int(nextItem.x - 1, nextItem.y));
                    continue;
                }
                if (_map.GetMapItemAt(nextItem.x + 1, nextItem.y).distance == i)
                {
                    pathBack.Add(new Vector3Int(nextItem.x + 1, nextItem.y));
                    continue;
                }
                if (_map.GetMapItemAt(nextItem.x, nextItem.y + 1).distance == i)
                {
                    pathBack.Add(new Vector3Int(nextItem.x, nextItem.y + 1));
                    continue;
                }
                if (_map.GetMapItemAt(nextItem.x - 1, nextItem.y - 1).distance == i)
                {
                    pathBack.Add(new Vector3Int(nextItem.x - 1, nextItem.y - 1));
                    continue;
                }
                if (_map.GetMapItemAt(nextItem.x + 1, nextItem.y - 1).distance == i)
                {
                    pathBack.Add(new Vector3Int(nextItem.x + 1, nextItem.y - 1));
                    continue;
                }
                if (_map.GetMapItemAt(nextItem.x - 1, nextItem.y + 1).distance == i)
                {
                    pathBack.Add(new Vector3Int(nextItem.x - 1, nextItem.y + 1));
                    continue;
                }
                if (_map.GetMapItemAt(nextItem.x + 1, nextItem.y + 1).distance == i)
                {
                    pathBack.Add(new Vector3Int(nextItem.x + 1, nextItem.y + 1));
                    continue;
                }
            }
            return pathBack;
        }
    }
}
