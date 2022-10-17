using System;
using System.Collections.Generic;
using ObjectComponents;
using UnityEngine;

namespace Item.Models
{
    public class ItemObjectModel : BaseObjectModel
    {
        public string itemName { get; set; }
        public bool moveOnSpawn { get; set; }
        public eItemType itemType { get; set; }
        public eItemState itemState { get; set; }
        public eItemCategory itemCategory { get; set; }
        public decimal availableMass { get { return this.mass - this.claimedMass; } }
        public decimal claimedMass { get { return this.claimedMass_; } set { this.claimedMass_ = Math.Max(0, value); } }
        private decimal claimedMass_;
        public ItemObjectModel(Vector3Int _position, ItemObjectMass _item, eItemState _itemState, bool _moveOnSpawn) : base(_position, new List<ItemObjectMass> { _item })
        {
            ItemStatsModel itemStats = ItemStatsLibrary.GetItemStats(_item.itemType);
            this.itemState = _itemState;
            this.itemType = _item.itemType;
            this.itemName = itemStats.itemName;
            this.objectDescription = itemStats.itemDescription;
            this.itemCategory = itemStats.itemCategory;
            this.moveOnSpawn = _moveOnSpawn;
        }

        public ItemObjectModel RemoveMass(decimal massToSplitOff)
        {
            decimal newMass = this.mass - massToSplitOff;
            if (newMass <= 0) return this;
            this.GetObjectComponent<ObjectCompositionComponent>().RemoveMass(this.itemType, massToSplitOff);
            return new ItemObjectModel(this.position, new ItemObjectMass(this.itemType, massToSplitOff), eItemState.OnCharacter, false);
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

