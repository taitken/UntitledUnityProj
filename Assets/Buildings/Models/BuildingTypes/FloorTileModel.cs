using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class FloorTileModel : BuildingObjectModel
    {
        public FloorTileModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats) : base(_position, _buildingType, _buildStats)
        {

        }
    }
}

