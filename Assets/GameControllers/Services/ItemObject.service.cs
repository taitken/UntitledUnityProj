using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Item.Models;
using Unit.Models;
using Environment;
using Item;
using static Item.Models.ItemObjectModel;

namespace GameControllers.Services
{
    public class ItemObjectService : BaseService, IItemObjectService
    {
        private Func<IList<ItemObject>> ItemObjectHook;
        public Obseravable<IList<ItemObjectModel>> itemObseravable { get; set; } = new Obseravable<IList<ItemObjectModel>>(new List<ItemObjectModel>());
        public Obseravable<ItemObjectModel> onItemStoreOrSupplyTrigger { get; set; } = new Obseravable<ItemObjectModel>(null);
        public Obseravable<ItemObjectModel> onItemPickupOrDropTrigger { get; set; } = new Obseravable<ItemObjectModel>(null);

        public void SetItemObjectHook(Func<IList<ItemObject>> _itemObjectHook)
        {
            this.ItemObjectHook = _itemObjectHook;
        }
        public void AddItem(ItemObjectModel item)
        {
            IList<ItemObjectModel> _items = this.itemObseravable.Get();
            if (item != null && _items.Find(existingitem => { return item.ID == existingitem.ID; }) == null)
            {
                _items.Add(item);
                this.itemObseravable.Set(_items);
            }
        }

        public void RemoveItem(long id)
        {
            this.itemObseravable.Set(this.itemObseravable.Get().Filter(order => { return order.ID != id; }));
        }

        public bool IsItemAvailable(eItemType _itemType)
        {
            return this.GetAvailableItems(_itemType).Count > 0;
        }

        public ItemObjectModel FindClosestItem(eItemType _itemType, Vector3Int _startingPos)
        {
            IList<ItemObjectModel> itemObjs = this.GetAvailableItems(_itemType);
            long? lowestDistance = null;
            ItemObjectModel returnModel = null;
            itemObjs.ForEach(item =>
            {
                long distance = Math.Abs(_startingPos.x - item.position.x) + Math.Abs(_startingPos.y - item.position.y);
                if ((lowestDistance == null || distance < lowestDistance)
                 && item.claimedMass < item.mass)
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

        public IList<ItemObjectModel> GetAvailableItems(eItemType _itemType)
        {
            return this.itemObseravable.Get().Filter(item =>
            {
                return item.itemType == _itemType &&
                        (item.itemState == eItemState.OnGround || item.itemState == eItemState.InStorage);
            });
        }
    }
}
