using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public enum eWallTypes
    {
        Stone
    }
    public class WallBuildingModel : BuildingObjectModel
    {
        public eWallTypes wallType;
        public WallBuildingModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats) : base(_position, _buildingType, _buildStats)
        {
            this.wallType = _buildStats.wallType;
        }
    }
}

