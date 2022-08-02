using System.Collections;
using System.Collections.Generic;
using GameControllers.Services;
using Building.Models;
using UI.Services;
using UtilityClasses;
using UI.Models;
using GameControllers.Models;
using Item.Models;

namespace Building
{
    public struct ProductionSupplyOrder
    {
        public eItemType itemType;
        public BuildSupplyOrderModel currentBuildSupplyModel;
    }
    public class ProductionBuildingObject : BuildingObject
    {
        public ProductionBuildingModel productionBuildingModel;
        public IList<ProductionSupplyOrder> currentBuildSupplyModels;
        public override void Initialise(IContextWindowService _contextService,
                                        BuildingObjectModel _buildingObjectModel,
                                        IEnvironmentService _environmentService,
                                        IUnitOrderService _orderService)
        {
            base.Initialise(_contextService, _buildingObjectModel, _environmentService, _orderService);
            this.productionBuildingModel = _buildingObjectModel as ProductionBuildingModel;
            this.subscriptions.Add(this.unitOrderService.orders.Subscribe(this.CheckIfOrderRemoved));
            this.currentBuildSupplyModels = new List<ProductionSupplyOrder>();
            this.productionBuildingModel.productionSupplyMax.ForEach(supplyItem =>
            {
                this.currentBuildSupplyModels.Add(new ProductionSupplyOrder() { itemType = supplyItem.itemType, currentBuildSupplyModel = null });
            });
        }

        public void Update()
        {
            this.currentBuildSupplyModels.ForEach(supplyOrder =>
            {
                if (supplyOrder.currentBuildSupplyModel == null)
                {
                    BuildingSupply maxBuildSupply = this.productionBuildingModel.productionSupplyMax.Find(supply => { return supply.itemType == supplyOrder.itemType; });
                    ItemObjectModel currentSupply = this.productionBuildingModel.productionSupplyCurrent.Find(supply => { return supply.itemType == supplyOrder.itemType; });
                    if (currentSupply.mass < maxBuildSupply.mass / 2)
                    {
                        this.unitOrderService.AddOrder(new ProductionSupplyOrderModel(this.buildingObjectModel.position, maxBuildSupply.itemType, maxBuildSupply.mass - currentSupply.mass));
                    }
                }
            });
        }

        public void CheckIfOrderRemoved(IList<UnitOrderModel> unitOrders)
        {
            this.currentBuildSupplyModels.ForEach(supplyOrder =>
            {
                if (supplyOrder.currentBuildSupplyModel != null && !unitOrders.Any(order => { return order.ID == supplyOrder.currentBuildSupplyModel.ID; }))
                {
                    supplyOrder.currentBuildSupplyModel = null;
                }
            });
        }

        protected override List<string> GenerateContextWindowBody()
        {
            List<string> newContext = base.GenerateContextWindowBody();
            newContext.Add("Produces other items");
            newContext.Add(((int)this.productionBuildingModel.productionSupplyMax[0].mass).ToString() + " " + LocalisationDict.mass);
            return newContext;
        }
    }
}
