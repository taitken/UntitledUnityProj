using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;
using Unit.Models;

namespace GameControllers.Services
{
    public class UnitOrderService : BaseService, IUnitOrderService
    {
        public MonoObseravable<MouseActionModel> mouseAction { get; set; } = new MonoObseravable<MouseActionModel>(new MouseActionModel(eMouseAction.None));
        public MonoObseravable<UnitOrderModel> hideOrderIconTrigger { get; set; } = new MonoObseravable<UnitOrderModel>(null);
        public MonoObseravable<IList<UnitOrderModel>> orders { get; set; } = new MonoObseravable<IList<UnitOrderModel>>(new List<UnitOrderModel>());

        public void AddOrder(UnitOrderModel order)
        {
            IList<UnitOrderModel> _orders = this.orders.Get();
            if (order != null && order.IsUniqueCheck(_orders))
            {
                _orders.Add(order);
                this.orders.Set(_orders);
            }
        }

        public void RemoveOrder(long id)
        {
            this.orders.Set(this.orders.Get().Filter(order => { return order.ID != id; }));
        }

        public bool IsExistingOrderAtLocation(Vector3Int _location)
        {
            return this.orders.Get().Map(order =>{return order.coordinates;}).Any(orderPos =>{return orderPos == _location;});
        }
    }
}
