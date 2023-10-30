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
        ICropService cropService;
        Tilemap tilemap;
        Func<bool> completeCondition { get; set; }

        public ActionFactory(IPathFinderService _pathFinderService,
                             IEnvironmentService _environmentService,
                             IUnitOrderService _orderService,
                             IBuildingService _buildingService,
                             IItemObjectService _itemService,
                             ICropService _cropService)
        {
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
            this.orderService = _orderService;
            this.cropService = _cropService;
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
                        .Then(() => { return new DigAction(_unit, this.pathFinderService, this.environmentService); });
                    break;
                case eOrderTypes.Build:
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new MoveAction(_unit, _unit.currentOrder.coordinates, this.pathFinderService, this.environmentService, true))
                        .Then(() => { return new BuildAction(_unit, this.buildingService); });
                    break;
                case eOrderTypes.CropPlant:
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new MoveAction(_unit, _unit.currentOrder.coordinates, this.pathFinderService, this.environmentService, true))
                        .Then(() => { return new PlantSeedAction(_unit, this.cropService, this.itemService); });
                    break;
                case eOrderTypes.BuildSupply:
                    BuildSupplyOrderModel buildSupplyOrder = _unit.currentOrder as BuildSupplyOrderModel;
                    ItemObjectModel bsItem = this.itemService.FindClosestItem(buildSupplyOrder.itemType, _unit.position);
                    if (NullItemCheck(bsItem, buildSupplyOrder)) break;
                    decimal bsMassToClaim = this.itemService.DetermineMassToPickup(_unit, bsItem, buildSupplyOrder.itemMass);
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new ClaimItemAction(_unit, bsItem, this.itemService, bsMassToClaim))
                        .Then(() => { return new SplitBuildSupplyAction(_unit, this.orderService, new List<ItemObjectModel> { bsItem }, bsMassToClaim); })
                        .Then(() => { return new MoveAction(_unit, bsItem.position, this.pathFinderService, this.environmentService, false); })
                        .Then(() => { return new PickupItemAction(_unit, this.itemService, this.buildingService, bsItem, bsMassToClaim); })
                        .Then(() => { return new MoveAction(_unit, buildSupplyOrder.coordinates, this.pathFinderService, this.environmentService, true); })
                        .Then(() => { return new BuildSupplyAction(_unit, this.buildingService, this.itemService, this.orderService); });
                    break;
                case eOrderTypes.Supply:
                    SupplyOrderModel supplyOrder = _unit.currentOrder as SupplyOrderModel;
                    ItemObjectModel psItem = this.itemService.FindClosestItem(supplyOrder.itemType, _unit.position);
                    if (NullItemCheck(psItem, supplyOrder)) break;
                    decimal psMassToClaim = this.itemService.DetermineMassToPickup(_unit, psItem, supplyOrder.itemMass);
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new ClaimItemAction(_unit, psItem, this.itemService, psMassToClaim))
                        .Then(() => { return new MoveAction(_unit, psItem.position, this.pathFinderService, this.environmentService, false); })
                        .Then(() => { return new PickupItemAction(_unit, this.itemService, this.buildingService, psItem, psMassToClaim); })
                        .Then(() => { return new MoveAction(_unit, supplyOrder.coordinates, this.pathFinderService, this.environmentService, true); })
                        .Then(() => { return new StoreAction(_unit, this.itemService, this.buildingService, supplyOrder.objectToSupply); });
                    break;
                case eOrderTypes.Store:
                    StoreOrderModel storeOrder = _unit.currentOrder as StoreOrderModel;
                    Vector3Int coordinates = _unit.currentOrder.coordinates;
                    if (NullItemCheck(storeOrder.itemModel, storeOrder)) break;
                    decimal sMassToClaim = this.itemService.DetermineMassToPickup(_unit, storeOrder.itemModel);
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new ClaimItemAction(_unit, storeOrder.itemModel, this.itemService, sMassToClaim))
                        .Then(() => { return new MoveAction(_unit, _unit.currentOrder.coordinates, this.pathFinderService, this.environmentService, false); })
                        .Then(() => { return new PickupItemAction(_unit, this.itemService, this.buildingService, storeOrder.itemModel, sMassToClaim); })
                        .Then(() => { return new DeleteOrderIconAction(_unit, this.orderService); })
                        .Then(() => { return new CreateNewStoreOrderAction(_unit, coordinates, this.orderService, this.itemService); })
                        .Then(() => { return new MoveAction(_unit, this.buildingService.GetClosestStorage(_unit.position).position, this.pathFinderService, this.environmentService, true); })
                        .Then(() => { return new StoreAction(_unit, this.itemService, this.buildingService, this.buildingService.GetClosestStorage(_unit.position)); });
                    break;
                case eOrderTypes.Deconstruct:
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new MoveAction(_unit, _unit.currentOrder.coordinates, this.pathFinderService, this.environmentService, true))
                        .Then(() => { return new DeconstructAction(_unit, this.buildingService); });
                    break;
                case eOrderTypes.CropHarvest:
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new MoveAction(_unit, _unit.currentOrder.coordinates, this.pathFinderService, this.environmentService, true))
                        .Then(() => { return new CropHarvestAction(_unit, this.cropService, this.itemService); });
                    break;
                case eOrderTypes.CropRemove:
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder, new MoveAction(_unit, _unit.currentOrder.coordinates, this.pathFinderService, this.environmentService, true))
                        .Then(() => { return new CropHarvestAction(_unit, this.cropService, this.itemService); })
                        .Then(() => { return new RemoveSeedAction(_unit, this.cropService, this.itemService); });
                    break;
                case eOrderTypes.Wander:
                    newSequence = new ActionSequence(this.orderService, _unit.currentOrder,
                        new MoveAction(_unit, _unit.currentOrder.coordinates, this.pathFinderService, this.environmentService, false));
                    break;
            }
            return newSequence;
        }

        private bool NullItemCheck(ItemObjectModel item, UnitOrderModel order)
        {
            if (item == null)
            {
                this.orderService.RemoveOrder(order.ID);
            }
            return item == null;
        }
    }
}