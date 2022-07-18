using System;
using UnityEngine;

namespace Item.Models
{
    public class ItemObjectModel : BasePhysicalObjectModel
    {
        public ItemObjectModel(Vector3Int _position, decimal _weight, eItemState _itemState) :base(_position, _weight)
        {
            this.itemState = _itemState;
        }

        public eItemState itemState {get;set;}

        public enum eItemState{
            OnGround,
            OnCharacter,
            InStorage
        }
    }
}

