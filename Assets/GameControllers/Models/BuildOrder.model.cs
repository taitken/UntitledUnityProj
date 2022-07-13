using System;
using UnityEngine;
using Building.Models;

namespace GameControllers.Models
{
    public class BuildOrderModel : UnitOrderModel
    {
        public eBuildingType buildingType;
        public BuildOrderModel(Vector3Int _coordinates, eBuildingType _buildingType) : base(_coordinates, eMouseAction.Build)
        {
            this.buildingType = _buildingType;
        }
    }
}

