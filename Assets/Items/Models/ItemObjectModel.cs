using System;
using UnityEngine;

namespace Item.Models
{
    public class ItemObjectModel : BasePhysicalObjectModel
    {
        public ItemObjectModel(Vector3Int _position, decimal _weight, eItemType _itemType, eItemState _itemState) : base(_position, _weight)
        {
            this.itemState = _itemState;
            this.itemType = _itemType;
        }
        public eItemType itemType { get; set; }
        public eItemState itemState { get; set; }

        public enum eItemState
        {
            OnGround,
            OnCharacter,
            InStorage,
            InSupply
        }
    }
}

