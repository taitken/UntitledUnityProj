using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public enum eDoorTypes
    {
        Stone
    }
    public class DoorBuildingModel : BuildingObjectModel
    {
        public eDoorTypes doorType;
        public DoorBuildingModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats) : base(_position, _buildingType, _buildStats)
        {
            this.doorType = _buildStats.doorType;
        }
    }
}

