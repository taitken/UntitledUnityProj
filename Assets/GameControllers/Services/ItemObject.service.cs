using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;
using Item.Models;

namespace GameControllers.Services
{
    public class ItemObjectService : IItemObjectService
    {
        public Subscribable<IList<ItemObjectModel>> itemSubscribable { get; set; } = new Subscribable<IList<ItemObjectModel>>(new List<ItemObjectModel>());

        public void AddItem(ItemObjectModel unit)
        {
            IList<ItemObjectModel> _units = this.itemSubscribable.Get();
            if (_units.Find(existingUnit => { return unit.ID == existingUnit.ID; }) == null)
            {
                _units.Add(unit);
                this.itemSubscribable.Set(_units);
            }
        }

        public void RemoveItem(long id)
        {
            this.itemSubscribable.Set(this.itemSubscribable.Get().Filter(order => { return order.ID != id; }));
        }
    }
}
