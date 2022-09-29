using System;
using UnityEngine;
using GameControllers.Models;
using GameControllers.Services;
using Building.Models;
using Building;
using Unit.Models;
using Item.Models;
using System.Collections.Generic;

namespace UnitAction
{
    public class SupplyAction : IUnitAction
    {
        private IBuildingService buildingService;
        private IItemObjectService itemObjectService;
        private SupplyOrderModel buildOrder;
        private IUnitOrderService unitOrderService;
        public UnitModel unit { get; set; }
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public SupplyAction(UnitModel _unit,
                          IBuildingService _buildingService,
                          IItemObjectService _itemObjectService,
                          IUnitOrderService _unitOrderService)
        {
            this.unit = _unit;
            this.buildingService = _buildingService;
            this.itemObjectService = _itemObjectService;
            this.unitOrderService = _unitOrderService;
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
            if (this.unit.carriedItem == null)
            {
                this.cancel = true;
                Debug.LogException(new System.Exception("Production supply action failed. Unit not carrying item."));
            }
            else
            {
                SupplyOrderModel supplyOrder = this.unit.currentOrder as SupplyOrderModel;
                ProductionBuildingModel buildingModel = this.buildingService.buildingObseravable.Get()
                    .Find(building => { return building.position == supplyOrder.coordinates && building.buildingType != eBuildingType.FloorTile; }) as ProductionBuildingModel;

                if (buildingModel.SupplyItem(this.unit.carriedItem)) this.itemObjectService.RemoveItemFromWorld(this.unit.carriedItem.ID);
                this.itemObjectService.onItemStoreOrSupplyTrigger.Set(this.unit.carriedItem);
                this.completed = true;
            }
            return true;
        }
    }
}