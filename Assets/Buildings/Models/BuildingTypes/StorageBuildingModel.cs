using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class StorageBuildingModel : BuildingObjectModel
    {
        public StorageBuildingModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats)
            : base(_position, _buildingType, _buildStats)
        {
            this.storedItems = new List<ItemObjectModel>();
            this.storageMax = _buildStats.storageMax;
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
