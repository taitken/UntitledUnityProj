using System;
using System.Collections.Generic;
using ObjectComponents;
using UnityEngine;

namespace Item.Models
{
    public class ItemObjectModel : BaseObjectModel
    {

        public eItemType itemType { get; set; }
        public eItemState itemState { get; set; }
        public decimal availableMass { get { return this.mass - this.claimedMass; } }
        public decimal claimedMass { get { return this.claimedMass_; } set { this.claimedMass_ = Math.Max(0, value); } }
        private decimal claimedMass_;
        public ItemObjectModel(Vector3Int _position, ItemObjectMass _item, eItemState _itemState) : base(_position, new List<ItemObjectMass> { _item })
        {
            this.itemState = _itemState;
            this.itemType = _item.itemType;
        }

        public ItemObjectModel RemoveMass(decimal massToSplitOff)
        {
            decimal newMass = this.mass - massToSplitOff;
            if (newMass <= 0) return this;
            this.GetObjectComponent<ObjectCompositionComponent>().RemoveMass(this.itemType, massToSplitOff);
            return new ItemObjectModel(this.position, new ItemObjectMass(this.itemType, massToSplitOff), eItemState.OnCharacter);
        }

        public void AddMass(decimal newMass)
        {
            this.GetObjectComponent<ObjectCompositionComponent>().AddMass(this.itemType, newMass);
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

