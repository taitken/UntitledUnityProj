using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;
using Unit.Models;

namespace GameControllers.Services
{
    public interface IUnitOrderService : IBaseService
    {
        MonoObseravable<MouseActionModel> mouseAction { get; set; }
        public MonoObseravable<IList<UnitOrderModel>> orders { get; set; }
        void OnOrderIconDeleteTrigger(MonoBehaviour2 behaviour, Action<UnitOrderModel> trigger);
        void HideOrderIcon(UnitOrderModel unitOrder);
        void OnOrderIconHideTrigger(MonoBehaviour2 behaviour, Action<UnitOrderModel> trigger);
        void DeleteOrderIcon(UnitOrderModel unitOrder);
        void AddOrder(UnitOrderModel order);
        IList<UnitOrderModel> GetOrders() ;
        IList<T> GetOrders<T>() where T : UnitOrderModel;
        void RemoveOrder(long id);
        bool IsExistingOrderAtLocation(Vector3Int _location);
    }
}

