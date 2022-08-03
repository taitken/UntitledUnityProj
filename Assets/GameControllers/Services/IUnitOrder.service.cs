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
        MonoObseravable<UnitOrderModel> hideOrderIconTrigger { get; set; }
        void AddOrder(UnitOrderModel order);
        void RemoveOrder(long id);
        bool IsExistingOrderAtLocation(Vector3Int _location);
    }
}

