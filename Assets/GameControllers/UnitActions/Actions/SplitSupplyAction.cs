
using System.Diagnostics;
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
        private decimal maxMassInTrip { get; set; }
        private SupplyOrderModel originalSupplyOrder { get; set; }
        public SplitSupplyAction(UnitModel _unit,
                          IUnitOrderService _orderService,
                          IList<ItemObjectModel> _itemsToCollect)
        {
            this.orderService = _orderService;
            this.unit = _unit;
            this.originalSupplyOrder = _unit.currentOrder as SupplyOrderModel;
            this.itemsToCollect = _itemsToCollect;
            decimal availableMass = 0;
            this.itemsToCollect.ForEach(item => { availableMass += (item.mass - item.claimedMass); });
            this.maxMassInTrip = Math.Min(Math.Min(originalSupplyOrder.itemMass, availableMass), this.unit.maxCarryWeight);
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
            if (this.unit.currentOrder is SupplyOrderModel)
            {

                if (this.maxMassInTrip < originalSupplyOrder.itemMass)
                {
                    this.orderService.AddOrder(originalSupplyOrder.SplitOrder(this.maxMassInTrip));
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