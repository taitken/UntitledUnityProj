using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;

namespace GameControllers.Services
{
    public interface IUnitActionService
    {
        Observable<IList<UnitActionModel>> actionQueue { get; set; }

        void addAction(UnitActionModel action);
    }
}

