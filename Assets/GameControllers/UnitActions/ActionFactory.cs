using System;
using System.Collections.Generic;
using GameControllers.Models;
using GameControllers.Services;
using Item.Models;
using Unit.Models;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnitAction
{

    public class ActionFactory
    {
        IPathFinderService pathFinderService;
        IEnvironmentService environmentService;
        IUnitOrderService orderService;
        IBuildingService buildingService;
        IItemObjectService itemService;
        Tilemap tilemap;
        Func<bool> completeCondition { get; set; }

        public ActionFactory(IPathFinderService _pathFinderService,
                             IEnvironmentService _environmentService,
                             IUnitOrderService _orderService,
                             IBuildingService _buildingService,
                             IItemObjectService _itemService)
        {
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
            this.orderService = _orderService;
            this.buildingService = _buildingService;
            this.itemService = _itemService;
        }

        public ActionSequence CreateSequence(UnitModel _unit)
        {
            ActionSequence newSequence = null;
            switch (_unit.currentOrder.orderType)
            {
                case eOrderTypes.Dig:
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new MoveAction(_unit, _unit.currentOrder.coordinates, this.pathFinderService, this.environmentService, true))
                        .Then(new DigAction(_unit, this.pathFinderService, this.environmentService));
                    break;
                case eOrderTypes.Build:
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new MoveAction(_unit, _unit.currentOrder.coordinates, this.pathFinderService, this.environmentService, true))
                        .Then(new BuildAction(_unit, this.buildingService));
                    break;
                case eOrderTypes.BuildSupply:
                    BuildSupplyOrderModel buildSupplyOrder = _unit.currentOrder as BuildSupplyOrderModel;
                    ItemObjectModel itemToSupply = this.itemService.FindClosestItem(buildSupplyOrder.itemType, this.environmentService.tileMapRef.LocalToCell(_unit.position));
                    if (itemToSupply == null)
                    {
                        this.orderService.RemoveOrder(buildSupplyOrder.ID);
                        break;
                    }
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new SplitBuildSupplyAction(_unit, this.orderService, new List<ItemObjectModel> { itemToSupply }))
                        .Then(new MoveAction(_unit, itemToSupply.position, this.pathFinderService, this.environmentService, false))
                        .Then(new PickupItemAction(_unit, this.itemService, this.buildingService, itemToSupply, buildSupplyOrder.itemMass))
                        .Then(new MoveAction(_unit, buildSupplyOrder.coordinates, this.pathFinderService, this.environmentService, true))
                        .Then(new BuildSupplyAction(_unit, this.buildingService, this.itemService, this.orderService));
                    break;
                case eOrderTypes.ProductionSupply:
                    ProductionSupplyOrderModel productionSupplyOrder = _unit.currentOrder as ProductionSupplyOrderModel;
                    ItemObjectModel itemToSupplyProduction = this.itemService.FindClosestItem(productionSupplyOrder.itemType, this.environmentService.tileMapRef.LocalToCell(_unit.position));
                    if (itemToSupplyProduction == null)
                    {
                        this.orderService.RemoveOrder(productionSupplyOrder.ID);
                        break;
                    }
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, 
                              new MoveAction(_unit, itemToSupplyProduction.position, this.pathFinderService, this.environmentService, false))
                        .Then(new PickupItemAction(_unit, this.itemService, this.buildingService, itemToSupplyProduction, productionSupplyOrder.itemMass))
                        .Then(new MoveAction(_unit, productionSupplyOrder.coordinates, this.pathFinderService, this.environmentService, true))
                        .Then(new ProductionSupplyAction(_unit, this.buildingService, this.itemService, this.orderService));
                    break;
                case eOrderTypes.Store:
                    StoreOrderModel storeOrder = _unit.currentOrder as StoreOrderModel;
                    if (storeOrder.itemModel == null)
                    {
                        this.orderService.RemoveOrder(storeOrder.ID);
                        break;
                    }
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new MoveAction(_unit, _unit.currentOrder.coordinates, this.pathFinderService, this.environmentService, false))
                        .Then(new PickupItemAction(_unit, this.itemService, this.buildingService, storeOrder.itemModel, _unit.maxCarryWeight))
                        .Then(new HideOrderIconAction(_unit, this.orderService))
                        .Then(new MoveAction(_unit, this.buildingService.GetClosestStorage(this.environmentService.tileMapRef.LocalToCell(_unit.position)).position, this.pathFinderService, this.environmentService, true))
                        .Then(new StoreAction(_unit, this.itemService, this.buildingService, this.buildingService.GetClosestStorage(this.environmentService.tileMapRef.LocalToCell(_unit.position))));
                    break;
            }
            return newSequence;
        }
    }
}