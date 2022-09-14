

using System.Collections.Generic;
using Item.Models;

namespace ObjectComponents
{
    public class ObjectCompositionComponent : ObjectComponent
    {
        private IList<ItemObjectMass> objectComposition;
        public ObjectCompositionComponent(IList<ItemObjectMass> _objectComposition) : base()
        {
            this.objectComposition = _objectComposition;
        }

        public IList<ItemObjectMass> GetComposition()
        {
            return this.objectComposition;
        }

        public ItemObjectMass GetComposition(eItemType _itemType)
        {
            return this.objectComposition.Find(comp => { return comp.itemType == _itemType; });
        }

        public decimal GetMass()
        {
            return this.objectComposition.Sum(item => { return item.mass; });
        }

        public decimal GetMass(eItemType _itemType)
        {
            return this.objectComposition.Filter(item => { return item.itemType == _itemType; }).Sum(item => { return item.mass; });
        }
        public void AddMass(eItemType _itemType, decimal _massToAdd)
        {
            int itemIndex = -1;
            for (int i = 0; i < this.objectComposition.Count; i++)
            {
                if (this.objectComposition[i].itemType == _itemType)
                {
                    itemIndex = i;
                    break;
                }
            }
            if (itemIndex > -1)
            {
                this.objectComposition[itemIndex] = new ItemObjectMass(_itemType, this.objectComposition[itemIndex].mass + _massToAdd);
            }
        }

        public void RemoveMass(eItemType _itemType, decimal _massToRemove)
        {
            int itemIndex = -1;
            for (int i = 0; i < this.objectComposition.Count; i++)
            {
                if (this.objectComposition[i].itemType == _itemType)
                {
                    itemIndex = i;
                    break;
                }
            }
            if (itemIndex > -1)
            {
                this.objectComposition[itemIndex] = new ItemObjectMass(_itemType, this.objectComposition[itemIndex].mass - _massToRemove);
            }
        }

    }
}
