using System;
using UnityEngine;

namespace Building.Models
{
    public class BuildingObjectModel : BaseModel
    {
        public BuildingObjectModel(Vector3Int _position, eBuildingType _buildingType) : base()
        {
            this.position = _position;
            this.buildingType = _buildingType;
        }
        public eBuildingType buildingType;
        public Vector3Int position { get; set; }
    }
}

