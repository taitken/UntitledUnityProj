using System;
using UnityEngine;
using GameControllers.Models;
using GameControllers.Services;
using Building.Models;
using Building;
using Unit.Models;
using Item.Models;

namespace UnitAction
{
    public class SupplyAction : IUnitAction
    {
        private UnitModel unit;
        private IBuildingService buildingService;
        private IItemObjectService itemObjectService;
        private BuildOrderModel buildOrder;
        private BuildingObjectFactory buildingFactory;
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public SupplyAction(UnitModel _unit,
                          IBuildingService _buildingService,
                          IItemObjectService _itemObjectService)
        {
            this.unit = _unit;
            this.buildingService = _buildingService;
            this.buildOrder = _unit.currentOrder as BuildOrderModel;
            this.itemObjectService = _itemObjectService;
        }

        public bool CheckCompleted()
        {
            return this.completed;
        }
        public bool PerformAction()
        {
            if (this.unit.carriedItem == null)
            {
                this.cancel = true;
            }
            else
            {
                SupplyOrderModel supplyOrder = this.unit.currentOrder as SupplyOrderModel;
                ItemObjectModel itemModel = this.unit.carriedItem;
                BuildSiteModel buildSiteModel = new BuildSiteModel(supplyOrder.coordinates, supplyOrder.buildingType);
                this.buildingService.AddBuildSite(buildSiteModel);
                buildSiteModel.SupplyItem(itemModel);
                itemModel.itemState = ItemObjectModel.eItemState.InSupply;
                itemModel.position = buildSiteModel.position;
                this.itemObjectService.GetItemObject(itemModel.ID)?.Destroy();
                this.unit.carriedItem = null;
                this.itemObjectService.onItemStoreTrigger.Set(this.unit.carriedItem);
                this.completed = true;
            }

            return true;
        }
    }
}