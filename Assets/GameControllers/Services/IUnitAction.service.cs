using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;

namespace GameControllers.Services
{
    public interface IUnitActionService
    {
        Subscribable<IList<UnitActionModel>> actionQueue { get; set; }

        void addAction(UnitActionModel action);
    }
}

