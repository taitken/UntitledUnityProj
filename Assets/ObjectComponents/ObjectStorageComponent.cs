

using System.Collections.Generic;
using Item.Models;

namespace ObjectComponents
{
    public class ObjectStorage : ObjectComponent
    {
        private IList<ItemObjectModel> storedItems { get; set; }
        public ObjectStorage() : base()
        {
            this.storedItems = new List<ItemObjectModel>();
        }

        public void AddItem(ItemObjectModel item)
        {
            this.storedItems.Add(item);
        }

        public void AddItem(IList<ItemObjectModel> items)
        {
            items.ForEach(item => { this.AddItem(item); });
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
        }

        public void RemoveItem(IList<ItemObjectModel> items)
        {
            items.ForEach(item => { this.RemoveItem(item); });
        }

    }
}
