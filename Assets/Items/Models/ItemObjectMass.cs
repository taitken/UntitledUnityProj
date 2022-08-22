
namespace Item.Models
{
    public struct ItemObjectMass
    {
        public ItemObjectMass(eItemType _itemType, decimal _mass)
        {
            this.itemType = _itemType;
            this.mass = _mass;
        }
        public eItemType itemType {get;set;}
        public decimal mass {get;set;}
    }
}
