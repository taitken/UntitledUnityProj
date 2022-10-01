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
        public GrowerBuildingModel growerBuildingModel;
        public CropHarvestOrderModel(Vector3Int _coordinates, CropObjectModel _cropObjectModel, GrowerBuildingModel _growerBuildingModel, bool showIcon = false) : base(_coordinates, eOrderTypes.CropHarvest, showIcon)
        {
            this.cropObjectModel = _cropObjectModel;
            this.growerBuildingModel = _growerBuildingModel;
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

