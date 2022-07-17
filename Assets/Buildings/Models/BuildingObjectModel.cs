using System;
using UnityEngine;

namespace Building.Models
{
    public class BuildingObjectModel : BasePhysicalObjectModel
    {
        public BuildingObjectModel(Vector3Int _position, eBuildingType _buildingType) : base(_position, 0)
        {
            this.buildingType = _buildingType;
        }
        public eBuildingType buildingType;
    }
}

