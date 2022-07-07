using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;

namespace GameControllers.Services
{
    public interface IUnitService
    {
        public Subscribable<IList<UnitModel>> unitSubscribable { get; set; }
        void AddUnit(UnitModel unit);
        void RemoveUnit(long id);
    }
}

