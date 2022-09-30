using System;
using UnityEngine;
using GameControllers.Models;
using GameControllers.Services;
using Building.Models;
using Building;
using Unit.Models;
using System.Collections.Generic;
using Crops.Models;
using Item.Models;
using ObjectComponents;

namespace UnitAction
{
    public class CropHarvestAction : IUnitAction
    {
        private ICropService cropService;
        private CropObjectModel cropObject;
        private GrowerBuildingModel growerBuildingModel;
        private IItemObjectService itemService;
        public UnitModel unit { get; set; }
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public CropHarvestAction(UnitModel _unit,
                                ICropService _cropService,
                                IItemObjectService _itemService)
        {
            this.unit = _unit;
            this.cropService = _cropService;
            this.itemService = _itemService;
            if (_unit.currentOrder is CropHarvestOrderModel)
            {
                CropHarvestOrderModel cropOrder = _unit.currentOrder as CropHarvestOrderModel;
                this.cropObject = cropOrder.cropObjectModel;
            }
            if (_unit.currentOrder is CropRemoveOrderModel)
            {
                CropRemoveOrderModel cropOrder = _unit.currentOrder as CropRemoveOrderModel;
                this.cropObject = cropOrder.growerBuilding.cropObject;
                this.growerBuildingModel = cropOrder.growerBuilding;
            }
        }

        public bool CheckCompleted()
        {
            return this.completed;
        }

        public void CancelAction()
        {
            this.cancel = true;
        }
        public bool PerformAction()
        {
            CropObjectModel cropObj = this.cropObject;
            if (cropObj == null)
            {
                this.cancel = true;
                Debug.LogException(new System.Exception("Harvest crop action failed. Crop not found"));
            }
            else
            {
                this.cropService.RemoveCrop(this.cropObject.ID);
                if (this.growerBuildingModel != null)
                {
                    this.growerBuildingModel.cropObject = null;
                    this.growerBuildingModel.NotifyModelUpdate();
                }
                this.completed = true;
            }
            return true;
        }
    }
}