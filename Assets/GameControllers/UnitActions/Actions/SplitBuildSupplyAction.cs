
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
    public class SplitBuildSupplyAction : IUnitAction
    {
        private IUnitOrderService orderService;
        private UnitModel unit;
        private IList<ItemObjectModel> itemsToCollect;
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        private decimal massToPickup { get; set; }
        private BuildSupplyOrderModel originalSupplyOrder { get; set; }
        public SplitBuildSupplyAction(UnitModel _unit,
                          IUnitOrderService _orderService,
                          IList<ItemObjectModel> _itemsToCollect,
                          decimal _massToPickup)
        {
            this.originalSupplyOrder = _unit.currentOrder as BuildSupplyOrderModel;
            this.itemsToCollect = _itemsToCollect;
            this.orderService = _orderService;
            this.unit = _unit;
            this.massToPickup = _massToPickup;
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
            if (this.unit.currentOrder is BuildSupplyOrderModel)
            {

                if (this.massToPickup < originalSupplyOrder.itemMass)
                {
                    this.orderService.AddOrder(originalSupplyOrder.SplitOrder(this.massToPickup));
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