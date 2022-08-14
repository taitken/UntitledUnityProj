using System;
using System.Collections.Generic;
using Item.Models;
using ObjectComponents;
using UnityEngine;

namespace Building.Models
{
    public class DecorBuildingModel : BuildingObjectModel
    {
        public DecorBuildingModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats)
            : base(_position, _buildingType, _buildStats)
        {

        }
    }
}
