
using System;
using UnityEngine;
using System.Collections.Generic;
using Environment.Models;
using GameControllers.Models;
using GameControllers.Services;
using Item.Models;
using Unit.Models;

namespace UnitAction
{
    public class PickupItemAction : IUnitAction
    {
        private UnitModel unit;
        private IPathFinderService pathFinderService;
        private IItemObjectService itemObjectService;
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public PickupItemAction(UnitModel _unit,
                          IItemObjectService _itemObjectService)
        {
            this.unit = _unit;
            this.itemObjectService = _itemObjectService;
        }

        public bool CheckCompleted()
        {
            return this.unit.carriedItem != null;
        }
        public bool PerformAction()
        {
            ItemObjectModel itemObjectModel = this.itemObjectService.itemObseravable.Get().Find(item =>{return item.position == this.unit.currentOrder.coordinates;});
            this.unit.carriedItem = itemObjectModel;
            this.itemObjectService.onItemPickupOrDropTrigger.NotifyAllSubscribers();
            return true;
        }
    }
}