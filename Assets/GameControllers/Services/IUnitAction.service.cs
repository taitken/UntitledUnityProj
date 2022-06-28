using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;

namespace GameControllers.Services
{
    public interface IUnitActionService
    {
        Subscribable<eMouseAction> mouseAction {get;set;}
        Subscribable<IList<UnitActionModel>> actionQueue { get; set; }

        void addAction(UnitActionModel action);
    }
}

