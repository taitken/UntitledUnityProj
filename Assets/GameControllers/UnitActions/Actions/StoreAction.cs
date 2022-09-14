
using System;
using UnityEngine;
using System.Collections.Generic;
using Environment.Models;
using GameControllers.Models;
using GameControllers.Services;
using Item.Models;
using Unit.Models;
using Building.Models;
using ObjectComponents;

namespace UnitAction
{
    public class StoreAction : IUnitAction
    {
        private IBuildingService buidlingService;
        private IItemObjectService itemObjectService;
        private BaseObjectModel objectToStore;
        public UnitModel unit { get; set; }
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public StoreAction(UnitModel _unit,
                          IItemObjectService _itemObjectService,
                          IBuildingService _buidlingService,
                          BaseObjectModel _objectToStore)
        {
            this.unit = _unit;
            this.itemObjectService = _itemObjectService;
            this.buidlingService = _buidlingService;
            this.objectToStore = _objectToStore;
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
            else if (this.objectToStore.GetObjectComponent<ObjectStorageComponent>() == null)
            {
                this.cancel = true;
                Debug.LogException(new System.Exception("Store action failed. Selected object has no storage component."));
            }
            else
            {
                ItemObjectModel itemModel = this.unit.carriedItem;
                itemModel.itemState = ItemObjectModel.eItemState.InStorage;
                itemModel.position = this.objectToStore.position;
                // Supply building
                ItemObjectModel existingStoredItem = this.objectToStore.GetObjectComponent<ObjectStorageComponent>().GetItem(itemModel.itemType);
                if (existingStoredItem != null)
                {
                    existingStoredItem.AddMass(itemModel.mass);
                    this.itemObjectService.RemoveItem(itemModel.ID);
                }
                else
                {
                    this.objectToStore.GetObjectComponent<ObjectStorageComponent>().AddItem(itemModel);
                }
                this.itemObjectService.onItemStoreOrSupplyTrigger.Set(this.unit.carriedItem);
                this.completed = true;
            }
            return true;
        }
    }
}