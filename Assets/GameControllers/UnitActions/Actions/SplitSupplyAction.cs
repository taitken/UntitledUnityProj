
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
    public class SplitSupplyAction : IUnitAction
    {
        private IUnitOrderService orderService;
        private UnitModel unit;
        private IList<ItemObjectModel> itemsToCollect;
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public SplitSupplyAction(UnitModel _unit,
                          IUnitOrderService _orderService,
                          IList<ItemObjectModel> _itemsToCollect)
        {
            this.orderService = _orderService;
            this.unit = _unit;
            this.itemsToCollect = _itemsToCollect;
        }

        public bool CheckCompleted()
        {
            return this.completed;
        }
        public bool PerformAction()
        {
            if (this.unit.currentOrder is SupplyOrderModel)
            {
                SupplyOrderModel originalSupplyOrder = this.unit.currentOrder as SupplyOrderModel;
                decimal availableMass = 0;
                this.itemsToCollect.ForEach(item =>{availableMass += item.mass;});
                decimal maxMassInTrip = Math.Min(Math.Min(originalSupplyOrder.itemMass, availableMass), this.unit.maxCarryWeight);
                if(maxMassInTrip < originalSupplyOrder.itemMass)
                {
                    this.orderService.AddOrder(originalSupplyOrder.SplitOrder(maxMassInTrip));
                }
                this.completed = true;
            }
            else
            {
                this.cancel = true;
            }
            return true;
        }
    }
}