
using System;
using System.Collections.Generic;
using Building.Models;
using GameControllers.Services;
using Item.Models;
using Unit.Models;
using UnityEngine;

namespace UnitAction
{
    public class PickupItemAction : IUnitAction
    {
        private UnitModel unit;
        private IPathFinderService pathFinderService;
        private IItemObjectService itemObjectService;
        private IBuildingService buildingService;
        private ItemObjectModel itemObjModel;
        private decimal massToPickup;
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
            this.massToPickup = Math.Min(_massToPickup, _itemObjModel.mass - _itemObjModel.claimedMass);;
            this.itemObjModel.claimedMass += this.massToPickup;
            this.itemObjectService.itemObseravable.SubscribeQuietly(items =>
            {
                if(!items.Any(item => { return item.ID == this.itemObjModel.ID; })) this.CancelAction();
            });
        }

        public bool CheckCompleted()
        {
            if(this.completed)
            {
                this.unclaimMass();
                return true;
            }
            return false;
        }

        public void CancelAction()
        {
            this.unclaimMass();
            this.cancel = true;
        }

        private void unclaimMass()
        {
            this.itemObjModel.claimedMass -= this.massToPickup;
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
                    itemToAttach = this.itemObjModel.SplitItemModel(this.massToPickup);
                    this.unit.carriedItem = itemToAttach;
                    this.itemObjectService.AddItem(itemToAttach);
                }
                else if (originState == ItemObjectModel.eItemState.InStorage)
                {
                    this.itemObjectService.RemoveItem(itemToAttach.ID);
                    this.buildingService.buildingObseravable.Get()
                        .Map(building => { return building as StorageBuildingModel; })
                        .Find(building => { return building.position == this.itemObjModel.position; })
                        .RemoveItem(itemToAttach.ID);
                    this.unit.carriedItem = itemToAttach;
                    this.itemObjectService.AddItem(itemToAttach);
                }
                this.unit.carriedItem = itemToAttach;
                this.itemObjectService.onItemPickupOrDropTrigger.NotifyAllSubscribers();
                this.completed = true;
            }
            return true;
        }
    }
}