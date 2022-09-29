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
        private MonoObseravable<UnitOrderModel> hideOrderIconTrigger { get; set; } = new MonoObseravable<UnitOrderModel>(null);
        private MonoObseravable<UnitOrderModel> deleteOrderIconTrigger { get; set; } = new MonoObseravable<UnitOrderModel>(null);
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
        public IList<UnitOrderModel> GetOrders()
        {
            return this.orders.Get();
        }

        public IList<T> GetOrders<T>() where T : UnitOrderModel
        {
            return this.orders.Get().Filter(order => { return order.orderType is T; }).Map(order => { return order as T; });
        }

        public void RemoveOrder(long id)
        {
            this.orders.Set(this.orders.Get().Filter(order => { return order.ID != id; }));
        }

        public void OnOrderIconHideTrigger(MonoBehaviour2 behaviour, Action<UnitOrderModel> trigger)
        {
            this.hideOrderIconTrigger.SubscribeQuietly(behaviour, trigger);
        }

        public void HideOrderIcon(UnitOrderModel unitOrder)
        {
            this.hideOrderIconTrigger.Set(unitOrder);
        }

        public void OnOrderIconDeleteTrigger(MonoBehaviour2 behaviour, Action<UnitOrderModel> trigger)
        {
            this.deleteOrderIconTrigger.SubscribeQuietly(behaviour, trigger);
        }

        public void DeleteOrderIcon(UnitOrderModel unitOrder)
        {
            unitOrder.iconDeletedFromWorld = true;
            unitOrder.coordinates = default(Vector3Int);
            this.deleteOrderIconTrigger.Set(unitOrder);
        }

        public bool IsExistingOrderAtLocation(Vector3Int _location)
        {
            return this.orders.Get().Map(order => { return order.coordinates; }).Any(orderPos => { return orderPos == _location; });
        }
    }
}
