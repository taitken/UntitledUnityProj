using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;

namespace GameControllers.Services
{
    public interface IPathFinderService : IBaseService
    {
        void SetPathFinderMap(PathFinderMap newMap);
        // Returns true if tile is impassable
        bool CheckIfImpassable(Vector3Int pos);
        Subscription OnPathFinderMapUpdate(MonoBehaviour2 subscribingObj, Action<PathFinderMap> action);
        PathFinderMap GetPathFinderMap();
        IList<Vector3Int> FindPath(Vector3Int startingPos, Vector3Int endPos, bool adjacentToEndPos);
        IList<Vector3Int> FindPath(Vector3Int startingPos, Vector3Int endPos, PathFinderMap _pathFinderMap, bool adjacentToEndPos);
        bool CanPathTo(Vector3Int startingPos, Vector3Int endPos, bool adjacentToEndPos);
        bool CanPathTo(Vector3Int startingPos, Vector3Int endPos, PathFinderMap _pathFinderMap, bool adjacentToEndPos);
    }
}

