using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Item.Models;
using Unit.Models;

namespace GameControllers.Services
{
    public interface IItemObjectService
    {
        Subscribable<IList<ItemObjectModel>> itemSubscribable { get; set; }
        Subscribable<UnitModel> unitPickedUpItem { get; set; }
        Subscribable<UnitModel> unitItemDropped { get; set; }
        void AddItem(ItemObjectModel item);
        void RemoveItem(long id);
    }
}

