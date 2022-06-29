using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;

namespace GameControllers.Services
{
    public class UnitActionService : IUnitActionService
    {
        public Subscribable<eMouseAction> mouseAction { get; set; } = new Subscribable<eMouseAction>(eMouseAction.None);

        public Subscribable<IList<UnitActionModel>> actionQueue { get; set; } = new Subscribable<IList<UnitActionModel>>(new List<UnitActionModel>());
        public Subscribable<IList<UnitOrderModel>> orders { get; set; } = new Subscribable<IList<UnitOrderModel>>(new List<UnitOrderModel>());

        public void addAction(UnitActionModel action)
        {
            IList<UnitActionModel> _queue = this.actionQueue.Get();
            _queue.Add(action);
            this.actionQueue.Set(_queue);
        }

        public void addOrder(UnitOrderModel order)
        {
            IList<UnitOrderModel> _orders = this.orders.Get();
            if (_orders.Find(existingOrder => { return order.coordinates == existingOrder.coordinates; }) == null)
            {
                _orders.Add(order);
                this.orders.Set(_orders);
            }
        }
        public void removeOrder(long id)
        {
            this.orders.Set(this.orders.Get().Filter(order => { return order.ID != id; }));
        }
    }
}
