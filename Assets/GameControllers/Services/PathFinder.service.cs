using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;

namespace GameControllers.Services
{
    public class PathFinderService : BaseService, IPathFinderService
    {
        public MonoObseravable<PathFinderMap> pathFinderMap { get; set; } = new MonoObseravable<PathFinderMap>(new PathFinderMap(new List<IList<PathFinderMapItem>>()));

        public bool CanPathTo(Vector3Int startingPos, Vector3Int endPos, PathFinderMap _pathFinderMap, bool adjacentToEndPos)
        {
            return this.FindPath(startingPos, endPos, _pathFinderMap, adjacentToEndPos) != null ? true : false;
        }

        public IList<Vector3Int> FindPath(Vector3Int startingPos, Vector3Int endPos, PathFinderMap _map, bool adjacentToEndPos)
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
            if (adjacentToEndPos) returnMap = this.AdjustPathToBeAdjacent(returnMap, _map);
            this.pathFinderMap.Get().Refresh();
            return returnMap;
        }

        private List<PathFinderMapItem> IncrementNeigbours(PathFinderMapItem item, PathFinderMap _map)
        {
            List<PathFinderMapItem> neighbours = new List<PathFinderMapItem>();
            PathFinderMapItem left = _map.GetPassableMapItemAt(item.x - 1, item.y);
            PathFinderMapItem top = _map.GetPassableMapItemAt(item.x, item.y + 1);
            PathFinderMapItem right = _map.GetPassableMapItemAt(item.x + 1, item.y);
            PathFinderMapItem bottom = _map.GetPassableMapItemAt(item.x, item.y - 1);
            PathFinderMapItem topLeft = top != null && left != null ? _map.GetPassableMapItemAt(item.x - 1, item.y + 1) : null;
            PathFinderMapItem topRight = top != null && right != null ? _map.GetPassableMapItemAt(item.x + 1, item.y + 1) : null;
            PathFinderMapItem bottomleft = bottom != null && left != null ? _map.GetPassableMapItemAt(item.x - 1, item.y - 1) : null;
            PathFinderMapItem bottomRight = bottom != null && right != null ? _map.GetPassableMapItemAt(item.x + 1, item.y - 1) : null;
            neighbours.Add(left);
            neighbours.Add(top);
            neighbours.Add(right);
            neighbours.Add(bottom);
            neighbours.Add(topLeft);
            neighbours.Add(topRight);
            neighbours.Add(bottomleft);
            neighbours.Add(bottomRight);
            // Remove out of bounds (null) tiles, and items that have already been assigned a lower
            neighbours = (List<PathFinderMapItem>)neighbours.Filter(neighbour => { return neighbour != null; });
            neighbours = (List<PathFinderMapItem>)neighbours.Filter(neighbour => { return neighbour.distance == null || neighbour.distance > item.distance + 1; });
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

        private IList<Vector3Int> AdjustPathToBeAdjacent(IList<Vector3Int> path, PathFinderMap _map)
        {
            if (path == null || path.Count <= 1) return null;
            if ((path[path.Count - 1].x - path[path.Count - 2].x) != 0 && (path[path.Count - 1].y - path[path.Count - 2].y) != 0)
            {
                // Diagonal condition
                PathFinderMapItem newTile = _map.GetPassableMapItemAt(path[path.Count - 2].x + (path[path.Count - 1].x - path[path.Count - 2].x), path[path.Count - 2].y)
                    ?? _map.GetPassableMapItemAt(path[path.Count - 2].x, path[path.Count - 2].y + (path[path.Count - 1].y - path[path.Count - 2].y));
                if (newTile != null)
                {
                    path[path.Count - 1] = new Vector3Int(newTile.x, newTile.y);
                    if (path.Count >= 3 && (Math.Abs(path[path.Count - 1].x - path[path.Count - 3].x) + Math.Abs(path[path.Count - 1].y - path[path.Count - 3].y)) == 2)
                    {
                        path.RemoveAt(path.Count - 2);
                    }
                }
            }
            else
            {
                // Uni condition
                path.RemoveAt(path.Count - 1);
            }
            return path;
        }
    }
}
