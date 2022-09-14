using System;
using UnityEngine;
using Building.Models;
using Item.Models;
using System.Collections.Generic;
using Unit.Models;
using GameControllers.Services;

namespace GameControllers.Models
{
    public class SupplyOrderModel : UnitOrderModel
    {
        public eItemType itemType;
        public BaseObjectModel objectToSupply;
        public decimal itemMass;
        public SupplyOrderModel(BaseObjectModel _objectToSupply, eItemType _itemType, decimal _itemMass, bool _showOrderIcon = false) 
            : base(_objectToSupply.position, eOrderTypes.Supply, _showOrderIcon)
        {
            this.itemType = _itemType;
            this.itemMass = _itemMass;
            this.objectToSupply = _objectToSupply;
        }

        public SupplyOrderModel SplitOrder(decimal massToKeep)
        {
            decimal newMass = this.itemMass - massToKeep;
            if (newMass <= 0)
            {
                return null;
            }
            else
            {
                this.itemMass = massToKeep;
                return new SupplyOrderModel(this.objectToSupply, this.itemType, newMass,  false);
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

