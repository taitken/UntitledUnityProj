using System;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class ChestBuildingModel : StorageBuildingModel
    {
        public ChestBuildingModel(Vector3Int _position, eBuildingType _buildingType) : base(_position, _buildingType)
        {
            this.storageMax = 10000.00M;
            this.requiredItems.Add(new BuildingSupply(eItemType.Stone, 400));
        }
    }
}

