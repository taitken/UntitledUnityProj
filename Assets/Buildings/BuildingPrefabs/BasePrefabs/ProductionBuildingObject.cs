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
    public struct ProductionSupplyOrder
    {
        public ProductionSupplyOrder(eItemType _itemType, ProductionSupplyOrderModel _buildSupply)
        {
            itemType = _itemType;
            currentBuildSupplyModel = _buildSupply;
        }
        public eItemType itemType { get; set; }
        public ProductionSupplyOrderModel currentBuildSupplyModel { get; set; }
    }
    public class ProductionBuildingObject : BuildingObject
    {
        public ProductionBuildingModel productionBuildingModel;
        public IList<ProductionSupplyOrder> currentProductionSupplyModels;
        protected override void OnCreation()
        {
            this.productionBuildingModel = this.buildingObjectModel as ProductionBuildingModel;
            this.currentProductionSupplyModels = new List<ProductionSupplyOrder>();
            this.productionBuildingModel.productionSupplyMax.ForEach(supplyItem =>
            {
                this.currentProductionSupplyModels.Add(new ProductionSupplyOrder() { itemType = supplyItem.itemType, currentBuildSupplyModel = null });
            });
            this.unitOrderService.orders.Subscribe(this, this.CheckIfOrderRemoved);
        }

        public void Update()
        {
            this.CheckAndRequestSupply();
        }

        public void FixedUpdate()
        {
            this.Produce();
        }

        public override void OnSelect()
        {
            IList<BasePanelModel> panels = new List<BasePanelModel>();
            panels.Add(new ObjectPanelModel(this.buildingObjectModel.ID, this.buildingObjectModel.buildingType.ToString()));
            panels.Add(new RecipePanelModel(this.buildingObjectModel.ID, "Production Options"));
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
                this.productionBuildingModel.productionPointsCurrent += Time.fixedDeltaTime;
                if (this.productionBuildingModel.productionPointsCurrent >= this.productionBuildingModel.productionPointsMax)
                {
                    this.produceItem();
                    this.productionBuildingModel.productionPointsCurrent = this.productionBuildingModel.isFullySupplied ?
                                                                            this.productionBuildingModel.productionPointsCurrent - this.productionBuildingModel.productionPointsMax :
                                                                            0;
                }
            }
        }

        private void produceItem()
        {
            this.productionBuildingModel.productionSupplyCurrent.ForEach(item =>
            {
                BuildingSupply inputRequirement = this.productionBuildingModel.inputs.Find(input => { return input.itemType == item.itemType; });

                // Throw error if somehow an input was added where it shouldnt have been
                if (inputRequirement.Equals(default(BuildingSupply)))
                    throw (new Exception("Building attempting to produce output with invalid input item. Building type: "
                        + this.buildingObjectModel.buildingType.ToString() + ". " + "Invalid input type: " + item.itemType.ToString()));

                item.mass -= inputRequirement.mass;
                if (item.mass <= 0)
                {
                    this.itemService.RemoveItem(item.ID);
                }
            });
            this.productionBuildingModel.productionSupplyCurrent = this.productionBuildingModel.productionSupplyCurrent.Filter(supply => { return supply.mass > 0; });
            this.productionBuildingModel.outputs.ForEach(output =>
            {
                this.itemService.AddItem(new ItemObjectModel(this.productionBuildingModel.position, output.mass, output.itemType, ItemObjectModel.eItemState.OnGround));
            });
        }

        private void CheckAndRequestSupply()
        {
            this.currentProductionSupplyModels.ForEach((supplyModel, index) =>
            {
                if (supplyModel.currentBuildSupplyModel == null)
                {
                    BuildingSupply maxBuildSupply = this.productionBuildingModel.productionSupplyMax.Find(supply => { return supply.itemType == supplyModel.itemType; });
                    ItemObjectModel currentSupply = this.productionBuildingModel.productionSupplyCurrent.Find(supply => { return supply.itemType == supplyModel.itemType; });
                    if (currentSupply == null || currentSupply.mass < maxBuildSupply.mass / 2)
                    {
                        ProductionSupplyOrderModel newOrder = new ProductionSupplyOrderModel(this.buildingObjectModel.position, maxBuildSupply.itemType, maxBuildSupply.mass - (currentSupply == null ? 0 : currentSupply.mass));
                        this.currentProductionSupplyModels[index] = new ProductionSupplyOrder(supplyModel.itemType, newOrder);
                        this.unitOrderService.AddOrder(newOrder);
                    }
                }
            });
        }

        private void CheckIfOrderRemoved(IList<UnitOrderModel> unitOrders)
        {
            this.currentProductionSupplyModels.ForEach((supplyOrder, index) =>
            {
                if (supplyOrder.currentBuildSupplyModel != null && !unitOrders.Any(order => { return order.ID == supplyOrder.currentBuildSupplyModel.ID; }))
                {
                    this.currentProductionSupplyModels[index] = new ProductionSupplyOrder(supplyOrder.itemType, null);
                }
            });
        }

        protected override List<string> GenerateContextWindowBody()
        {
            List<string> newContext = base.GenerateContextWindowBody();
            newContext.Add("Produces other items");
            this.productionBuildingModel.productionSupplyMax.ForEach(productionMax =>
            {
                ItemObjectModel supplyCurrent = this.productionBuildingModel.productionSupplyCurrent.Find(item => { return item.itemType == productionMax.itemType; });
                newContext.Add(productionMax.itemType.ToString() + ": " +
                    (supplyCurrent != null ? supplyCurrent.mass : 0).ToString() + "/" +
                    ((int)productionMax.mass).ToString() + " " + LocalisationDict.mass);
            });

            return newContext;
        }
    }
}
