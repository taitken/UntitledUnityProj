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
        private ItemObjectLayer itemLayer { get; set; }
        public Obseravable<IList<ItemObjectModel>> itemObseravable { get; set; } = new Obseravable<IList<ItemObjectModel>>(new List<ItemObjectModel>());
        public Obseravable<UnitModel> unitPickedUpItem { get; set; } = new Obseravable<UnitModel>(null);
        public Obseravable<UnitModel> unitItemDropped { get; set; } = new Obseravable<UnitModel>(null);

        public void InstantiateLayer(ItemObjectLayer _itemLayer)
        {
            this.itemLayer = _itemLayer;
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
            return this.itemLayer.itemObjects.Find(item =>{return item.itemObjectModel.ID == id;});
        }
    }
}
