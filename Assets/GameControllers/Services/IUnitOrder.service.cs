using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;
using Unit.Models;

namespace GameControllers.Services
{
    public interface IUnitOrderService
    {
        Subscribable<MouseActionModel> mouseAction { get; set; }
        public Subscribable<IList<UnitOrderModel>> orders { get; set; }
        UnitOrderModel GetNextOrder(UnitModel requestingUnit);
        void AddOrder(UnitOrderModel order);
        void RemoveOrder(long id);
    }
}

