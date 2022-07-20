using System;
using UnityEngine;

namespace Item.Models
{
    public class ItemObjectModel : BasePhysicalObjectModel
    {

        public eItemType itemType { get; set; }
        public eItemState itemState { get; set; }
        public ItemObjectModel(Vector3Int _position, decimal _weight, eItemType _itemType, eItemState _itemState) : base(_position, _weight)
        {
            this.itemState = _itemState;
            this.itemType = _itemType;
        }

        public ItemObjectModel SplitItemModel(decimal massToKeep)
        {
            decimal newMass = this.mass - massToKeep;
            if(newMass <= 0) return null;
            this.mass = massToKeep;
            return new ItemObjectModel(this.position, newMass, this.itemType, this.itemState);
        }

        public enum eItemState
        {
            OnGround,
            OnCharacter,
            InStorage,
            InSupply
        }
    }
}

