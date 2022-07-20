using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Unit.Models;

namespace GameControllers.Services
{
    public class UnitService : BaseService, IUnitService
    {
        public UnitService()
        {
            
        }
        public Obseravable<IList<UnitModel>> unitObseravable { get; set; } = new Obseravable<IList<UnitModel>>(new List<UnitModel>());

        public void AddUnit(UnitModel unit)
        {
            IList<UnitModel> _units = this.unitObseravable.Get();
            if (unit != null && _units.Find(existingUnit => { return unit.ID == existingUnit.ID; }) == null)
            {
                _units.Add(unit);
                this.unitObseravable.Set(_units);
            }
        }

        public void RemoveUnit(long id)
        {
            this.unitObseravable.Set(this.unitObseravable.Get().Filter(order => { return order.ID != id; }));
        }
    }
}
