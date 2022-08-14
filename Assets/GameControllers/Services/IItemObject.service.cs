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
    public interface IItemObjectService : IBaseService
    {
        MonoObseravable<IList<ItemObjectModel>> itemObseravable { get; set; }
        MonoObseravable<ItemObjectModel> onItemPickupOrDropTrigger { get; set; }
        MonoObseravable<ItemObjectModel> onItemStoreOrSupplyTrigger { get; set; }
        void SetItemObjectHook(Func<IList<ItemObject>> _itemObjectHook);
        void AddItem(ItemObjectModel item);
        void RemoveItem(long id);
        ItemObjectModel FindClosestItem(eItemType _itemType, Vector3Int _startingPos);
        decimal DetermineMassToPickup(UnitModel unit, ItemObjectModel item);
        decimal DetermineMassToPickup(UnitModel unit, ItemObjectModel item, decimal massRequested);
        public bool IsItemAvailable(eItemType _itemType);
        public ItemObject GetItemObject(long id);
        public Sprite GetItemSprite(eItemType itemType);
    }
}

