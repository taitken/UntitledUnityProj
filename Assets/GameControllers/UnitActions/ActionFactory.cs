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
                case eOrderTypes.Supply:
                    SupplyOrderModel supplyOrder = _unit.currentOrder as SupplyOrderModel;
                    ItemObjectModel itemToSupply = this.itemService.FindClosestItem(supplyOrder.itemType, this.environmentService.tileMapRef.LocalToCell(_unit.position));
                    if(itemToSupply == null)
                    {
                        this.orderService.RemoveOrder(_unit.currentOrder.ID);
                        break;
                    }
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new MoveAction(_unit, itemToSupply.position, this.pathFinderService, this.environmentService, false))
                        .Then(new PickupItemAction(_unit, this.itemService, itemToSupply))
                        .Then(new MoveAction(_unit, supplyOrder.coordinates, this.pathFinderService, this.environmentService, true))
                        .Then(new SupplyAction(_unit, this.buildingService, this.itemService));
                    break;
                case eOrderTypes.Store:
                    ItemObjectModel itemToStore = this.itemService.itemObseravable.Get().Find(item =>{return item.position == _unit.currentOrder.coordinates;});
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new MoveAction(_unit, _unit.currentOrder.coordinates, this.pathFinderService, this.environmentService, false))
                        .Then(new PickupItemAction(_unit, this.itemService, itemToStore))
                        .Then(new HideOrderIconAction(_unit, this.orderService))
                        .Then(new MoveAction(_unit, this.buildingService.GetClosestStorage(this.environmentService.tileMapRef.LocalToCell(_unit.position)).position, this.pathFinderService, this.environmentService, true))
                        .Then(new StoreAction(_unit, this.itemService, this.buildingService, this.buildingService.GetClosestStorage(this.environmentService.tileMapRef.LocalToCell(_unit.position))));
                    break;
            }
            return newSequence;
        }
    }
}