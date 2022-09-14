

using System;
using System.Collections.Generic;
using Item.Models;
using UtilityClasses;

namespace ObjectComponents
{
    public class ObjectStorageComponent : ObjectComponent
    {
        private EventEmitter<IList<ItemObjectModel>> onItemsAddedEmitter = new EventEmitter<IList<ItemObjectModel>>();
        private EventEmitter<IList<ItemObjectModel>> onItemsRemovedEmitter = new EventEmitter<IList<ItemObjectModel>>();
        private IList<ItemObjectModel> storedItems { get; set; }
        public ObjectStorageComponent() : base()
        {
            this.storedItems = new List<ItemObjectModel>();
        }
        public void AddItem(ItemObjectModel item)
        {
            this.storedItems.Add(item);
            this.onItemsAddedEmitter.Emit(new List<ItemObjectModel>() { item });
        }
        public void AddItem(IList<ItemObjectModel> items)
        {
            items.ForEach(item => { this.AddItem(item); });
            this.onItemsAddedEmitter.Emit(items);
        }
        public void ListenForItemsAdded(Action<IList<ItemObjectModel>> itemsAdded)
        {
            this.onItemsAddedEmitter.OnEmit(itemsAdded);
        }
        public void ListenForItemsRemoved(Action<IList<ItemObjectModel>> itemsRemoved)
        {
            this.onItemsAddedEmitter.OnEmit(itemsRemoved);
        }
        public IList<ItemObjectModel> GetItems()
        {
            return this.storedItems;
        }
        public ItemObjectModel GetItem(eItemType itemType)
        {
            return this.storedItems.Find(item => { return item.itemType == itemType; });
        }
        public decimal GetMass()
        {
            return this.storedItems.Sum(item => { return item.mass; });
        }
        public decimal GetMass(eItemType itemType)
        {
            return this.storedItems.Sum(item => { return item.itemType == itemType ? item.mass : 0; });
        }
        public void RemoveItem(ItemObjectModel item)
        {
            this.storedItems.Remove(item);
            this.onItemsRemovedEmitter.Emit(new List<ItemObjectModel>() { item });
        }
        public void RemoveItem(IList<ItemObjectModel> items)
        {
            items.ForEach(item => { this.RemoveItem(item); });
            this.onItemsRemovedEmitter.Emit(items);
        }
    }
}
