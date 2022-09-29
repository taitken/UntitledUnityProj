using System;
using System.Collections.Generic;
using Building.Models;
using UtilityClasses;
using UI.Models;
using GameControllers.Models;
using Item.Models;
using UnityEngine;

namespace Building
{
    public class ProductionBuildingObject : BuildingObject
    {
        public ProductionBuildingModel productionBuildingModel;
        public IList<ProductionSupplyOrder> currentProductionSupplyModels;
        protected override void OnCreation()
        {
            this.productionBuildingModel = this.buildingObjectModel as ProductionBuildingModel;
            this.currentProductionSupplyModels = new List<ProductionSupplyOrder>();
            this.unitOrderService.orders.Subscribe(this, this.CheckIfOrderRemoved);
        }

        public void Update()
        {
            // Very Heavy. Need rework at some point to reduce cpu load. Move out of update.
            this.CheckCurrentRecipe();
            this.RefreshProductionSupplyModels();
            this.CheckAndRequestSupply();
        }

        public void FixedUpdate()
        {
            this.Produce();
        }

        public override void OnSelect()
        {
            IList<BasePanelModel> panels = new List<BasePanelModel>();
            panels.Add(new ObjectPanelModel(this.buildingObjectModel.ID, this.buildingObjectModel.buildingType.ToString(), this.productionBuildingModel));
            panels.Add(new RecipePanelModel(this.buildingObjectModel.ID, "Production Options", this.productionBuildingModel));
            this.uiPanelService.selectedObjectPanels.Set(panels);
        }

        public override void OnMouseEnter()
        {
            this.uiPanelService.AddContext(new ProductionBuildingContextWindowModel(this.buildingObjectModel.ID, this.GenerateContextWindowTitle(), this.productionBuildingModel));
        }

        private void Produce()
        {
            if (this.productionBuildingModel.isFullySupplied)
            {
                this.productionBuildingModel.selectedItemRecipe.productionPointsCurrent += Time.fixedDeltaTime;
                if (this.productionBuildingModel.selectedItemRecipe.productionPointsCurrent >= this.productionBuildingModel.selectedItemRecipe.productionPointsMax)
                {
                    this.ProduceItem();
                }
            }
        }

        private void ProduceItem()
        {
            this.productionBuildingModel.selectedItemRecipe.inputs.ForEach(input =>
            {
                ItemObjectModel itemObject = this.productionBuildingModel.buildingStorage.GetItem(input.itemType);
                itemObject.RemoveMass(input.mass);
                if (itemObject.mass <= 0)
                {
                    this.itemService.RemoveItemFromWorld(itemObject.ID);
                }
            });
            this.productionBuildingModel.buildingStorage.RemoveItem(this.productionBuildingModel.buildingStorage.GetItems().Filter(supply => { return supply.mass <= 0; }));
            this.productionBuildingModel.selectedItemRecipe.outputs.ForEach(output =>
            {
                this.itemService.AddItemToWorld(new ItemObjectModel(this.productionBuildingModel.position, output, ItemObjectModel.eItemState.OnGround));
            });
            AllocatedItemRecipe allocatedItem = this.productionBuildingModel.itemRecipes.Find(recipe => { return recipe.recipe == this.productionBuildingModel.selectedItemRecipe; });
            allocatedItem.counter--;
            this.productionBuildingModel.selectedItemRecipe.productionPointsCurrent = 0;
            this.productionBuildingModel.selectedItemRecipe = null;
        }

        private void CheckAndRequestSupply()
        {
            this.currentProductionSupplyModels.ForEach((supplyModel, index) =>
            {
                if (supplyModel.currentBuildSupplyModel == null)
                {
                    // Hard to interpet. 
                    ItemObjectMass input = this.productionBuildingModel.itemRecipes.Find(recipe =>
                    {
                        return recipe.counter > 0 && recipe.recipe.inputs.Any(input =>
                        {
                            return input.itemType == supplyModel.input.itemType;
                        });
                    }).recipe.inputs.Find(input => { return input.itemType == supplyModel.input.itemType; });
                    ItemObjectModel currentSupply = this.productionBuildingModel.buildingStorage.GetItem(supplyModel.input.itemType);
                    if (currentSupply == null || currentSupply.mass < (input.mass * ProductionBuildingModel.MAX_STORAGE_MULT) / 2)
                    {
                        SupplyOrderModel newOrder = new SupplyOrderModel(this.productionBuildingModel, input.itemType, (input.mass * ProductionBuildingModel.MAX_STORAGE_MULT) - (currentSupply == null ? 0 : currentSupply.mass));
                        this.currentProductionSupplyModels[index] = new ProductionSupplyOrder(newOrder, supplyModel.input);
                        this.unitOrderService.AddOrder(newOrder);
                    }
                }
            });
        }

        private void RefreshProductionSupplyModels()
        {
            IList<ItemObjectMass> requiredItems = new List<ItemObjectMass>();
            this.productionBuildingModel.itemRecipes.ForEach(recipe =>
            {
                if (recipe.counter > 0)
                {
                    requiredItems.AddRange(recipe.recipe.inputs);
                }
                else
                {
                    if (this.productionBuildingModel.selectedItemRecipe == recipe.recipe)
                    {
                        this.CancelCurrentRecipe();
                    }
                }
            });
            IList<ItemObjectMass> newItemsToRequest = requiredItems.Filter(input => { return !this.currentProductionSupplyModels.Any(model => { return model.input.itemType == input.itemType; }); });
            IList<ProductionSupplyOrder> ordersToRemove = this.currentProductionSupplyModels.Filter(order => { return !requiredItems.Any(input => { return input.itemType == order.input.itemType; }); });
            newItemsToRequest.ForEach(input => { this.currentProductionSupplyModels.Add(new ProductionSupplyOrder(null, input)); });
            this.currentProductionSupplyModels = this.currentProductionSupplyModels.Filter(order =>
            {
                return ordersToRemove == null || !ordersToRemove.Any(removeOrder => { return removeOrder.currentBuildSupplyModel == order.currentBuildSupplyModel; });
            });

        }

        private void CheckCurrentRecipe()
        {
            if (this.productionBuildingModel.selectedItemRecipe == null)
            {
                this.productionBuildingModel.selectedItemRecipe = this.GetNextRecipe(this.productionBuildingModel);
            }
        }

        private ItemRecipeModel GetNextRecipe(ProductionBuildingModel buildModel)
        {
            foreach (AllocatedItemRecipe recipe in buildModel.itemRecipes)
            {
                if (recipe.counter > 0)
                {
                    return recipe.recipe;
                }
            };
            return null;
        }

        private void CancelCurrentRecipe()
        {
            this.productionBuildingModel.selectedItemRecipe.productionPointsCurrent = 0;
            this.productionBuildingModel.selectedItemRecipe = null;
        }

        private void CheckIfOrderRemoved(IList<UnitOrderModel> unitOrders)
        {
            this.currentProductionSupplyModels.ForEach((supplyOrder, index) =>
            {
                if (supplyOrder.currentBuildSupplyModel != null && !unitOrders.Any(order => { return order.ID == supplyOrder.currentBuildSupplyModel.ID; }))
                {
                    this.currentProductionSupplyModels[index] = new ProductionSupplyOrder(null, supplyOrder.input);
                }
            });
        }

        protected override List<string> GenerateContextWindowBody()
        {
            List<string> newContext = base.GenerateContextWindowBody();
            newContext.Add("Produces other items");
            this.productionBuildingModel.selectedItemRecipe.inputs.ForEach(input =>
            {
                ItemObjectModel supplyCurrent = this.productionBuildingModel.buildingStorage.GetItem(input.itemType);
                newContext.Add(input.itemType.ToString() + ": " +
                    (supplyCurrent != null ? supplyCurrent.mass : 0).ToString() + "/" +
                    LocalisationDict.GetMassString(input.mass));
            });
            return newContext;
        }
    }
}
