using System;
using UnityEngine;
using Building.Models;
using System.Collections.Generic;
using Unit.Models;

namespace GameControllers.Models
{
    public class CropRemoveOrderModel : UnitOrderModel
    {
        public GrowerBuildingModel growerBuilding;
        public CropRemoveOrderModel(Vector3Int _coordinates, GrowerBuildingModel _growerBuilding, bool showIcon = false) : base(_coordinates, eOrderTypes.CropRemove, showIcon)
        {
            this.growerBuilding = _growerBuilding;
        }

        public override bool IsUniqueCheck(IList<UnitOrderModel> orderList)
        {
            return orderList.Find(existingOrder => { return this.ID == existingOrder.ID || (this.coordinates == existingOrder.coordinates && this.orderType == existingOrder.orderType); }) == null;
        }

        public override bool CanAssignToUnit(IList<IBaseService> _services, UnitModel _unitModel)
        {
            return base.CanAssignToUnit(_services, _unitModel);
        }
    }
}

