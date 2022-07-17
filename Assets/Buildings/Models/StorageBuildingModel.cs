using System;
using UnityEngine;

namespace Building.Models
{
    public class StorageBuildingModel : BuildingObjectModel
    {
        public StorageBuildingModel(Vector3Int _position, eBuildingType _buildingType) : base(_position, _buildingType)
        {
            
        }
        public decimal storageMax;
        public decimal storageCurrent;
    }
}

