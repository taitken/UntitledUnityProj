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
    public interface IItemObjectService
    {
        Obseravable<IList<ItemObjectModel>> itemObseravable { get; set; }
        Obseravable onItemPickupOrDropTrigger { get; set; }
        Obseravable<ItemObjectModel> onItemStoreTrigger { get; set; }
        void SetItemObjectHook(Func<IList<ItemObject>> _itemObjectHook);
        void AddItem(ItemObjectModel item);
        void RemoveItem(long id);
        public ItemObject GetItemObject(long id);
    }
}

