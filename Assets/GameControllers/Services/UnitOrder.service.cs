using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;
using Unit.Models;

namespace GameControllers.Services
{
    public class UnitOrderService : IUnitOrderService
    {
        public Obseravable<MouseActionModel> mouseAction { get; set; } = new Obseravable<MouseActionModel>(new MouseActionModel(eMouseAction.None));
        public Obseravable<UnitOrderModel> hideOrderIconTrigger { get; set; } = new Obseravable<UnitOrderModel>(null);
        public Obseravable<IList<UnitOrderModel>> orders { get; set; } = new Obseravable<IList<UnitOrderModel>>(new List<UnitOrderModel>());

        public UnitOrderModel GetNextOrder(UnitModel requestingUnit)
        {
            if (this.orders.Get().Count > 0)
            {
                UnitOrderModel nextOrder = null;
                this.orders.Get().ForEach(order =>
                {
                    bool exitBreak = false;
                    if (order.assignedUnit == null && exitBreak == false)
                    {
                        nextOrder = order;
                        exitBreak = true;
                    }
                });
                return nextOrder;
            }
            return null;
        }
        public void AddOrder(UnitOrderModel order)
        {
            IList<UnitOrderModel> _orders = this.orders.Get();
            if (_orders.Find(existingOrder => { return order.ID == existingOrder.ID; }) == null)
            {
                _orders.Add(order);
                this.orders.Set(_orders);
            }
        }

        public void RemoveOrder(long id)
        {
            this.orders.Set(this.orders.Get().Filter(order => { return order.ID != id; }));
        }
    }
}
