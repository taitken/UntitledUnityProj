using System;
using UnityEngine;
using Building.Models;

namespace GameControllers.Models
{
    public class BuildOrderModel : UnitOrderModel
    {
        public eBuildingType buildingType;
        public BuildOrderModel(Vector3Int _coordinates, eBuildingType _buildingType, bool showIcon = false) : base(_coordinates, eOrderTypes.Build, showIcon)
        {
            this.buildingType = _buildingType;
        }
    }
}

