using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;

namespace GameControllers.Services
{
    public class UnitService : IUnitService
    {
        public UnitService()
        {
            
        }
        public Subscribable<IList<UnitModel>> unitSubscribable { get; set; } = new Subscribable<IList<UnitModel>>(new List<UnitModel>());

        public void AddUnit(UnitModel unit)
        {
            IList<UnitModel> _units = this.unitSubscribable.Get();
            if (_units.Find(existingUnit => { return unit.ID == existingUnit.ID; }) == null)
            {
                _units.Add(unit);
                this.unitSubscribable.Set(_units);
            }
        }

        public void RemoveUnit(long id)
        {
            this.unitSubscribable.Set(this.unitSubscribable.Get().Filter(order => { return order.ID != id; }));
        }
    }
}
