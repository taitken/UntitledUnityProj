using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Item.Models;

namespace GameControllers.Services
{
    public interface IItemObjectService
    {
        Subscribable<IList<ItemObjectModel>> itemSubscribable { get; set; }
        void AddItem(ItemObjectModel item);
        void RemoveItem(long id);
    }
}

