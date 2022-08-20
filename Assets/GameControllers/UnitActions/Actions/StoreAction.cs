
using System;
using UnityEngine;
using System.Collections.Generic;
using Environment.Models;
using GameControllers.Models;
using GameControllers.Services;
using Item.Models;
using Unit.Models;
using Building.Models;

namespace UnitAction
{
    public class StoreAction : IUnitAction
    {
        private IBuildingService buidlingService;
        private IItemObjectService itemObjectService;
        private StorageBuildingModel buildingModel;
        public UnitModel unit { get; set; }
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public StoreAction(UnitModel _unit,
                          IItemObjectService _itemObjectService,
                          IBuildingService _buidlingService,
                          BuildingObjectModel _building)
        {
            this.unit = _unit;
            this.itemObjectService = _itemObjectService;
            this.buidlingService = _buidlingService;
            this.buildingModel = _building as StorageBuildingModel;
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
                Debug.LogException(new System.Exception("Store action failed. Unit not carrying item."));
            }
            else
            {
                ItemObjectModel itemModel = this.unit.carriedItem;
                itemModel.itemState = ItemObjectModel.eItemState.InStorage;
                itemModel.position = this.buildingModel.position;
                // Supply build site
                ItemObjectModel existingStoredItem = this.buildingModel.buildingStorage.GetItem(itemModel.itemType);
                if (existingStoredItem != null)
                {
                    existingStoredItem.MergeItemModel(itemModel.mass);
                    this.itemObjectService.RemoveItem(itemModel.ID);
                }
                else
                {
                    this.buildingModel.StoreItem(itemModel);
                }
                this.itemObjectService.onItemStoreOrSupplyTrigger.Set(this.unit.carriedItem);
                this.completed = true;
            }
            return true;
        }
    }
}