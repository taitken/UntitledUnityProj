using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class StorageBuildingModel : BuildingObjectModel
    {
        public StorageBuildingModel(Vector3Int _position, Vector2 _size, eBuildingType _buildingType, IList<BuildingSupply> _requiredItems, decimal _storageMax)
            : base(_position, _size, _buildingType, _requiredItems)
        {
            this.storedItems = new List<ItemObjectModel>();
            this.storageMax = _storageMax;
        }

        public decimal storageMax { get; set; }
        public decimal storageCurrent
        {
            get
            {
                decimal storageCurrent = 0;
                this.storedItems.ForEach(item => { storageCurrent += item.mass; });
                return storageCurrent;
            }
        }
        public IList<ItemObjectModel> storedItems { get; set; }
        public void StoreItem(ItemObjectModel itemObj)
        {
            this.storedItems.Add(itemObj);
        }

        public void RemoveItem(long itemObjID)
        {
            this.storedItems = this.storedItems.Filter(item => { return item.ID != itemObjID; });
        }
    }
}
