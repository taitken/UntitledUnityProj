using System;
using UnityEngine;
using Building.Models;
using Item.Models;
using System.Collections.Generic;
using Unit.Models;
using GameControllers.Services;

namespace GameControllers.Models
{
    public class BuildSupplyOrderModel : UnitOrderModel
    {
        public eItemType itemType;
        public eBuildingType buildingType;
        public decimal itemMass;
        public BuildSupplyOrderModel(Vector3Int _coordinates, eItemType _itemType, decimal _itemMass, eBuildingType _buildingType, bool _showBuildGhost = true) : base(_coordinates, eOrderTypes.BuildSupply, false, _showBuildGhost)
        {
            this.itemType = _itemType;
            this.itemMass = _itemMass;
            this.buildingType = _buildingType;
        }

        public BuildSupplyOrderModel SplitOrder(decimal massToKeep)
        {
            decimal newMass = this.itemMass - massToKeep;
            if (newMass <= 0)
            {
                return null;
            }
            else
            {
                this.itemMass = massToKeep;
                return new BuildSupplyOrderModel(this.coordinates, this.itemType, newMass, this.buildingType, false);
            }
        }

        public override bool IsUniqueCheck(IList<UnitOrderModel> orderList)
        {
            return orderList.Find(existingOrder => { return this.ID == existingOrder.ID; }) == null;
        }

        public override bool CanAssignToUnit(IList<IBaseService> _services, UnitModel _unitModel)
        {
            IItemObjectService itemService = this.GetService<IItemObjectService>(_services);
            if (itemService == null) return false;
            return base.CanAssignToUnit(_services, _unitModel) && itemService.IsItemAvailable(this.itemType);
        }
    }
}

