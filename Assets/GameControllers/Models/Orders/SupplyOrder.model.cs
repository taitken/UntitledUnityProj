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
        public decimal itemMass;
        public SupplyOrderModel(Vector3Int _coordinates, eItemType _itemType, decimal _itemMass, eBuildingType _buildingType, bool _showBuildGhost = true) : base(_coordinates, eOrderTypes.Supply, _showBuildGhost)
        {
            this.itemType = _itemType;
            this.itemMass = _itemMass;
            this.buildingType = _buildingType;
        }

        public SupplyOrderModel SplitOrder(decimal massToKeep)
        {
            decimal newMass = this.itemMass - massToKeep;
            if(newMass <= 0) return null;
            this.itemMass = massToKeep;
            return new SupplyOrderModel(this.coordinates, this.itemType, newMass, this.buildingType, false);
        }
    }
}

