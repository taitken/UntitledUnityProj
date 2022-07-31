using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class FloorTileModel : BuildingObjectModel
    {
        public FloorTileModel(Vector3Int _position, Vector2 _size, eBuildingType _buildingType, IList<BuildingSupply> _requiredItems) : base(_position, _size, _buildingType, _requiredItems)
        {

        }
    }
}

