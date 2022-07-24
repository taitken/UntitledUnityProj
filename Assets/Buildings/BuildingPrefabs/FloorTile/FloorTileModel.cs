using System;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class FloorTileModel : BuildingObjectModel
    {
        public FloorTileModel(Vector3Int _position, eBuildingType _buildingType) : base(_position, _buildingType)
        {
            this.requiredItems.Add(new BuildingSupply(eItemType.Stone, 50));
        }
    }
}

