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
        Obseravable<UnitModel> unitPickedUpItem { get; set; }
        void InstantiateLayer(ItemObjectLayer _itemLayer);
        void AddItem(ItemObjectModel item);
        void RemoveItem(long id);
        public ItemObject GetItemObject(long id);
    }
}

