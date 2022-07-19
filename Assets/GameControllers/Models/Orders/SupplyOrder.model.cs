using System;
using UnityEngine;
using Building.Models;
using Item.Models;

namespace GameControllers.Models
{
    public class SupplyOrderModel : UnitOrderModel
    {
        public eItemType itemType;
        public eBuildingType buildingType;
        public SupplyOrderModel(Vector3Int _coordinates, eItemType _itemType, eBuildingType _buildingType) : base(_coordinates, eOrderTypes.Supply)
        {
            this.itemType = _itemType;
            this.buildingType = _buildingType;
        }
    }
}

