using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class ProductionBuildingModel : BuildingObjectModel
    {
        public IList<BuildingSupply> productionSupplyMax { get; set; }
        public IList<ItemObjectModel> productionSupplyCurrent { get; set; }
        public float productionPointsMax { get; set; }
        public float productionPointsCurrent { get; set; }
        public IList<BuildingSupply> inputs { get; set; }
        public IList<BuildingSupply> outputs { get; set; }
        public bool isFullySupplied
        {
            get
            {
                bool suppliedItems = true;
                this.inputs.ForEach(requiredInput =>
                {
                    if (this.productionSupplyCurrent.Filter(item => { return item.itemType == requiredInput.itemType; }).Sum(item => { return item.mass; }) < requiredInput.mass)
                    {
                        suppliedItems = false;
                    }
                });
                return suppliedItems;
            }
        }
        public ProductionBuildingModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats)
            : base(_position, _buildingType, _buildStats)
        {
            this.productionSupplyCurrent = new List<ItemObjectModel>();
            this.productionSupplyMax = _buildStats.productionSupply;
            this.productionPointsMax = _buildStats.productionPointsMax;
            this.productionPointsCurrent = 0;
            this.inputs = _buildStats.inputs;
            this.outputs = _buildStats.outputs;
        }

        // Returns true if item is merged with existing item. Returns false if item is not merged.
        public bool SupplyItem(ItemObjectModel itemObject)
        {
            ItemObjectModel existingItem = this.productionSupplyCurrent.Find(supply => { return supply.itemType == itemObject.itemType; });
            if (existingItem != null)
            {
                existingItem.MergeItemModel(itemObject.mass);
                return true;
            }
            else
            {
                this.productionSupplyCurrent.Add(itemObject);
                itemObject.itemState = ItemObjectModel.eItemState.InSupply;
                return false;
            }
        }
    }
}

