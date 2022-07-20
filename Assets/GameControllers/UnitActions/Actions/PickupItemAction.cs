
using System.Collections.Generic;
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
        private ItemObjectModel itemObjModel;
        private decimal massToPickup;
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public PickupItemAction(UnitModel _unit,
                          IItemObjectService _itemObjectService,
                          ItemObjectModel _itemObjModel,
                          decimal _massToPickup)
        {
            this.unit = _unit;
            this.itemObjectService = _itemObjectService;
            this.itemObjModel = _itemObjModel;
            this.cancel = this.itemObjModel == null;
            this.massToPickup = _massToPickup;
            this.itemObjectService.itemObseravable.SubscribeQuietly(items =>
            {
                this.cancel = !items.Any(item => { return item.ID == this.itemObjModel.ID; });
            });
        }

        public bool CheckCompleted()
        {
            return this.completed;
        }
        public bool PerformAction()
        {
            if (this.itemObjModel == null)
            {
                this.cancel = true;
            }
            else
            {
                if (this.itemObjModel.mass > this.massToPickup)
                {
                    this.itemObjectService.AddItem(this.itemObjModel.SplitItemModel(this.massToPickup));
                }
                this.unit.carriedItem = this.itemObjModel;
                this.itemObjectService.onItemPickupOrDropTrigger.NotifyAllSubscribers();
                this.completed = true;
            }
            return true;
        }
    }
}