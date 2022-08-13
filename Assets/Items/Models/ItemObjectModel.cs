using System;
using UnityEngine;

namespace Item.Models
{
    public class ItemObjectModel : BaseObjectModel
    {

        public eItemType itemType { get; set; }
        public eItemState itemState { get; set; }
        public decimal claimedMass { get {return this.claimedMass_;} set {this.claimedMass_ = Math.Max(0, value);}}
        private decimal claimedMass_;
        public ItemObjectModel(Vector3Int _position, decimal _weight, eItemType _itemType, eItemState _itemState) : base(_position, _weight)
        {
            this.itemState = _itemState;
            this.itemType = _itemType;
        }

        public ItemObjectModel SplitItemModel(decimal massToSplitOff)
        {
            decimal newMass = this.mass - massToSplitOff;
            if (newMass <= 0) return this;
            this.mass = newMass;
            return new ItemObjectModel(this.position, massToSplitOff, this.itemType, eItemState.OnCharacter);
        }

        public void MergeItemModel(decimal newMass)
        {
            this.mass += newMass;
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

