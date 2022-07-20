using System;
using System.Collections.Generic;
using UnityEngine;

namespace Building.Models
{
    public class BuildingObjectModel : BasePhysicalObjectModel
    {        
        public eBuildingType buildingType { get; set; }
        public IList<BuildingSupply> requiredItems { get; set; }
        public BuildingObjectModel(Vector3Int _position, eBuildingType _buildingType) : base(_position, 0)
        {
            this.buildingType = _buildingType;
            this.requiredItems = new List<BuildingSupply>();
        }

    }
}

