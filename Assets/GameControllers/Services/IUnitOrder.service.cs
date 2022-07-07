using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;

namespace GameControllers.Services
{
    public interface IUnitOrderService
    {
        Subscribable<eMouseAction> mouseAction { get; set; }
        public Subscribable<IList<UnitOrderModel>> orders { get; set; }
        UnitOrderModel GetNextOrder(UnitModel requestingUnit);
        void AddOrder(UnitOrderModel order);
        void RemoveOrder(long id);
    }
}

