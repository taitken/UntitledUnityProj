using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Item.Models;
using Unit.Models;
using Environment;
using Item;

namespace GameControllers.Services
{
    public class ItemObjectService : IItemObjectService
    {
        private Func<IList<ItemObject>> ItemObjectHook;
        public Obseravable<IList<ItemObjectModel>> itemObseravable { get; set; } = new Obseravable<IList<ItemObjectModel>>(new List<ItemObjectModel>());
        public Obseravable onItemPickupTrigger { get; set; } = new Obseravable();

        public void SetItemObjectHook(Func<IList<ItemObject>> _itemObjectHook)
        {
            this.ItemObjectHook = _itemObjectHook;
        }
        public void AddItem(ItemObjectModel unit)
        {
            IList<ItemObjectModel> _units = this.itemObseravable.Get();
            if (_units.Find(existingUnit => { return unit.ID == existingUnit.ID; }) == null)
            {
                _units.Add(unit);
                this.itemObseravable.Set(_units);
            }
        }

        public void RemoveItem(long id)
        {
            this.itemObseravable.Set(this.itemObseravable.Get().Filter(order => { return order.ID != id; }));
        }

        
        public ItemObject GetItemObject(long id)
        {
            return this.ItemObjectHook().Find(item =>{return item.itemObjectModel.ID == id;});
        }
    }
}
