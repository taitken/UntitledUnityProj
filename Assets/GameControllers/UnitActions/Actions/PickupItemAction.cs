
using System;
using System.Collections.Generic;
using Building.Models;
using GameControllers.Services;
using Item.Models;
using Unit.Models;
using UnityEngine;
using UtilityClasses;

namespace UnitAction
{
    public class PickupItemAction : IUnitAction
    {
        private IPathFinderService pathFinderService;
        private IItemObjectService itemObjectService;
        private IBuildingService buildingService;
        private ItemObjectModel itemObjModel;
        private Subscription subscription;
        private decimal massToPickup;
        public UnitModel unit { get; set; }
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public PickupItemAction(UnitModel _unit,
                          IItemObjectService _itemObjectService,
                          IBuildingService _buildingService,
                          ItemObjectModel _itemObjModel,
                          decimal _massToPickup)
        {
            this.unit = _unit;
            this.itemObjectService = _itemObjectService;
            this.buildingService = _buildingService;
            this.itemObjModel = _itemObjModel;
            this.cancel = this.itemObjModel == null;
            this.massToPickup = _massToPickup; 
            this.subscription = this.itemObjectService.itemObseravable.SubscribeQuietly(null, items =>
            {
                if (!items.Any(item => { return item.ID == this.itemObjModel.ID; })) this.CancelAction();
            });
        }

        public bool CheckCompleted()
        {
            if (this.completed)
            {
                this.subscription.unsubscribe();
                return true;
            }
            return false;
        }

        public void CancelAction()
        {
            this.subscription.unsubscribe();
            this.cancel = true;
        }
        public bool PerformAction()
        {
            if (this.itemObjModel == null)
            {
                this.cancel = true;
            }
            else
            {
                ItemObjectModel itemToAttach = this.itemObjModel;
                ItemObjectModel.eItemState originState = this.itemObjModel.itemState;
                if (this.itemObjModel.mass > this.massToPickup)
                {
                    itemToAttach = this.itemObjModel.RemoveMass(this.massToPickup);
                    this.unit.carriedItem = itemToAttach;
                    this.itemObjectService.AddItemToWorld(itemToAttach);
                }
                else if (originState == ItemObjectModel.eItemState.InStorage)
                {
                    this.itemObjectService.RemoveItemFromWorld(itemToAttach.ID);
                    this.buildingService.buildingObseravable.Get()
                        .Map(building => { return building as StorageBuildingModel; })
                        .Find(building => { return building.position == this.itemObjModel.position; })
                        .RemoveItem(itemToAttach);
                    this.unit.carriedItem = itemToAttach;
                    this.itemObjectService.AddItemToWorld(itemToAttach);
                }
                this.unit.carriedItem = itemToAttach;
                this.itemObjectService.onItemPickupOrDropTrigger.NotifyAllSubscribers();
                this.completed = true;
            }
            return true;
        }
    }
}