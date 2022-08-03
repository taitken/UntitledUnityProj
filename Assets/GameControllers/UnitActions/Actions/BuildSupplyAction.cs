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
    public class BuildSupplyAction : IUnitAction
    {
        private UnitModel unit;
        private IBuildingService buildingService;
        private IItemObjectService itemObjectService;
        private BuildingObjectFactory buildingFactory;
        private IUnitOrderService unitOrderService;
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public BuildSupplyAction(UnitModel _unit,
                          IBuildingService _buildingService,
                          IItemObjectService _itemObjectService,
                          IUnitOrderService _unitOrderService)
        {
            this.unit = _unit;
            this.buildingService = _buildingService;
            this.itemObjectService = _itemObjectService;
            this.unitOrderService = _unitOrderService;
            this.buildingFactory = new BuildingObjectFactory();
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
            }
            else
            {
                BuildSupplyOrderModel supplyOrder = this.unit.currentOrder as BuildSupplyOrderModel;
                ItemObjectModel itemModel = this.unit.carriedItem;
                BuildSiteModel buildSiteModel = this.buildingService.buildingSiteObseravable.Get().Find(site => { return site.position == supplyOrder.coordinates; });
                // Create build sit where non-existant
                if (buildSiteModel == null)
                {
                    buildSiteModel = new BuildSiteModel(this.buildingFactory.CreateBuildingModel(supplyOrder.coordinates, supplyOrder.buildingType));
                    this.buildingService.AddBuildSite(buildSiteModel);
                }
                // Supply build site
                ItemObjectModel existingStoredItem = buildSiteModel.suppliedItems.Find(item => { return item.itemType == itemModel.itemType; });
                if (existingStoredItem != null)
                {
                    existingStoredItem.MergeItemModel(itemModel.mass);
                    this.itemObjectService.RemoveItem(itemModel.ID);
                }
                else
                {
                    buildSiteModel.SupplyItem(itemModel);
                }
                this.itemObjectService.onItemStoreOrSupplyTrigger.Set(this.unit.carriedItem);
                // Check if build site has all materials
                if (buildSiteModel.isFullySupplied)
                {
                    this.unitOrderService.RemoveOrder(supplyOrder.ID);
                    this.unitOrderService.AddOrder(new BuildOrderModel(buildSiteModel.position, buildSiteModel.buildingType));
                }
                this.completed = true;
            }
            return true;
        }
    }
}