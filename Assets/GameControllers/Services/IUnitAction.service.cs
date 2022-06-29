using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;

namespace GameControllers.Services
{
    public interface IUnitActionService
    {
        Subscribable<eMouseAction> mouseAction { get; set; }
        Subscribable<IList<UnitActionModel>> actionQueue { get; set; }
        public Subscribable<IList<UnitOrderModel>> orders { get; set; }
        void addAction(UnitActionModel action);
        void addOrder(UnitOrderModel order);
        void removeOrder(long id);
    }
}

