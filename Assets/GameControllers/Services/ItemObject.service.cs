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
        public Obseravable<ItemObjectModel> onItemStoreTrigger { get; set; } = new Obseravable<ItemObjectModel>(null);
        public Obseravable onItemPickupOrDropTrigger { get; set; } = new Obseravable();

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

        public ItemObjectModel FindClosestItem(eItemType _itemType, Vector3Int _startingPos)
        {
            IList<ItemObjectModel> itemObjs = this.itemObseravable.Get().Filter(item => { return item.itemType == _itemType; });
            long? lowestDistance = null;
            ItemObjectModel returnModel = null;
            itemObjs.ForEach(item =>
            {
                long distance = Math.Abs(_startingPos.x - item.position.x) + Math.Abs(_startingPos.y - item.position.y);
                if (lowestDistance == null || distance < lowestDistance)
                {
                    lowestDistance = distance;
                    returnModel = item;
                }
            });
            return returnModel;
        }
        public ItemObject GetItemObject(long id)
        {
            return this.ItemObjectHook().Find(item => { return item.itemObjectModel.ID == id; });
        }
    }
}
