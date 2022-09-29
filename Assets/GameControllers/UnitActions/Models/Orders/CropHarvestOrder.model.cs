using System;
using UnityEngine;
using Building.Models;
using System.Collections.Generic;
using Unit.Models;
using Crops.Models;

namespace GameControllers.Models
{
    public class CropHarvestOrderModel : UnitOrderModel
    {
        public CropObjectModel cropObjectModel;
        public CropHarvestOrderModel(Vector3Int _coordinates, CropObjectModel _cropObjectModel, bool showIcon = false) : base(_coordinates, eOrderTypes.CropHarvest, showIcon)
        {
            this.cropObjectModel = _cropObjectModel;
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

