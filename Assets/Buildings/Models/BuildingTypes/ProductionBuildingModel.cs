using System;
using System.Collections.Generic;
using Item.Models;
using ObjectComponents;
using UnityEngine;

namespace Building.Models
{

    [ObjectComponentAttribute(typeof(ObjectStorageComponent))]
    public class ProductionBuildingModel : BuildingObjectModel
    {
        public const int MAX_STORAGE_MULT = 10;
        public IList<AllocatedItemRecipe> itemRecipes { get; set; }
        public ItemRecipeModel selectedItemRecipe { get; set; }
        public ObjectStorageComponent buildingStorage { get { return this.GetObjectComponent<ObjectStorageComponent>(); } }
        public bool isFullySupplied
        {
            get
            {
                bool suppliedItems = true;
                if (this.selectedItemRecipe != null)
                {
                    this.selectedItemRecipe.inputs.ForEach(requiredInput =>
                    {
                        if (this.buildingStorage.GetItems().Filter(item => { return item.itemType == requiredInput.itemType; }).Sum(item => { return item.mass; }) < requiredInput.mass)
                        {
                            suppliedItems = false;
                        }
                    });
                }
                else
                {
                    return false;
                }
                return suppliedItems;
            }
        }

        public ProductionBuildingModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats)
                : base(_position, _buildingType, _buildStats)
        {
            this.itemRecipes = new List<AllocatedItemRecipe>();
            _buildStats.itemRecipes.ForEach(recipe => { this.itemRecipes.Add(new AllocatedItemRecipe(0, recipe)); });
        }

        // Returns true if item is merged with existing item. Returns false if item is not merged.
        public bool SupplyItem(ItemObjectModel itemObject)
        {
            ItemObjectModel existingItem = this.buildingStorage.GetItems().Find(supply => { return supply.itemType == itemObject.itemType; });
            if (existingItem != null)
            {
                existingItem.AddMass(itemObject.mass);
                return true;
            }
            else
            {
                this.buildingStorage.AddItem(itemObject);
                itemObject.itemState = ItemObjectModel.eItemState.InSupply;
                return false;
            }
        }
    }
}

