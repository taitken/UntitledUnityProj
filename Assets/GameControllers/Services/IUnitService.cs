using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Unit.Models;

namespace GameControllers.Services
{
    public interface IUnitService : IBaseService
    {
        public Obseravable<IList<UnitModel>> unitObseravable { get; set; }
        void AddUnit(UnitModel unit);
        void RemoveUnit(long id);
    }
}

