using System;
using System.Collections.Generic;
using Item.Models;
using ObjectComponents;
using UnityEngine;

namespace Building.Models
{
    [ObjectComponentAttribute(typeof(ObjectStorageComponent))]
    public class StorageBuildingModel : BuildingObjectModel
    {
        public decimal storageMax { get; set; }
        public ObjectStorageComponent buildingStorage { get { return this.GetObjectComponent<ObjectStorageComponent>(); } }
        public decimal storageCurrent { get { return this.buildingStorage.GetMass(); } }
        public StorageBuildingModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats)
            : base(_position, _buildingType, _buildStats)
        {
            this.storageMax = _buildStats.storageMax;
        }
        public void StoreItem(ItemObjectModel itemObj)
        {
            this.buildingStorage.AddItem(itemObj);
        }

        public void RemoveItem(ItemObjectModel itemObj)
        {
            this.buildingStorage.RemoveItem(itemObj);
        }
    }
}
