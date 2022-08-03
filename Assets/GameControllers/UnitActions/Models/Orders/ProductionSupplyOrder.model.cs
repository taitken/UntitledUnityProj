using System;
using UnityEngine;
using Building.Models;
using Item.Models;
using System.Collections.Generic;
using Unit.Models;
using GameControllers.Services;

namespace GameControllers.Models
{
    public class ProductionSupplyOrderModel : UnitOrderModel
    {
        public eItemType itemType;
        public eBuildingType buildingType;
        public decimal itemMass;
        public ProductionSupplyOrderModel(Vector3Int _coordinates, eItemType _itemType, decimal _itemMass, bool _showOrderIcon = false) 
            : base(_coordinates, eOrderTypes.ProductionSupply, _showOrderIcon)
        {
            this.itemType = _itemType;
            this.itemMass = _itemMass;
        }

        public ProductionSupplyOrderModel SplitOrder(decimal massToKeep)
        {
            decimal newMass = this.itemMass - massToKeep;
            if (newMass <= 0)
            {
                return null;
            }
            else
            {
                this.itemMass = massToKeep;
                return new ProductionSupplyOrderModel(this.coordinates, this.itemType, newMass, false);
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

