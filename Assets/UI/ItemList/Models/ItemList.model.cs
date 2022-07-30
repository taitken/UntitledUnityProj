

using System;
using Item.Models;
using UnityEngine;

namespace UI
{
    public class ItemListModel
    {
        public eItemType itemType { get; set; }
        public decimal mass { get; set; }
        public Sprite sprite { get; set; }

        public ItemListModel(eItemType _itemType, decimal _mass, Sprite _sprite)
        {
            this.itemType = _itemType;
            this.mass = _mass;
            this.sprite = _sprite;
        }
    }
}
