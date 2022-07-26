
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
    public class HideOrderIconAction : IUnitAction
    {
        private IUnitOrderService orderService;
        private UnitModel unit;
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public HideOrderIconAction(UnitModel _unit,
                          IUnitOrderService _orderService)
        {
            this.orderService = _orderService;
            this.unit = _unit;
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
            this.orderService.hideOrderIconTrigger.Set(this.unit.currentOrder);
            this.completed = true;
            return true;
        }
    }
}