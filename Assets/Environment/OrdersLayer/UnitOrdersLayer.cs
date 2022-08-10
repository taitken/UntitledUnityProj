using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;
using System;

namespace Environment
{
    public class UnitOrdersLayer : MonoBehaviourLayer
    {
        private IUnitOrderService orderService;
        private IEnvironmentService environmentService;
        private IList<UnitOrderModel> unitOrders;
        private IList<OrderIcon> orderIcons;
        private IList<OrderSelection> orderSelectionObjects;
        private OrderIcon.Factory orderIconFactory;
        private OrderSelection.Factory orderSelectionFactory;
        private Vector3Int lastEnteredCell;

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IEnvironmentService _environmentService,
                              OrderIcon.Factory _orderIconFactory,
                              LayerCollider.Factory _layerColliderFactory,
                              OrderSelection.Factory _orderSelectionFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "UnitOrderLayer");
            this.orderIconFactory = _orderIconFactory;
            this.orderSelectionFactory = _orderSelectionFactory;
            this.orderIcons = new List<OrderIcon>();
            this.unitOrders = new List<UnitOrderModel>();
            this.orderSelectionObjects = new List<OrderSelection>();
            this.orderService = _orderService;
            this.environmentService = _environmentService;
            this.orderService.orders.Subscribe(this, orders => { this.RecalculateOrders(orders); });
        }

        public override void OnDrag(DragEventModel dragEvent)
        {
            if (this.orderService.mouseAction.Get().mouseType == eMouseAction.Build && this.orderService.mouseAction.Get().buildingType == Building.Models.eBuildingType.FloorTile  ||
                this.orderService.mouseAction.Get().mouseType == eMouseAction.Dig ||
                this.orderService.mouseAction.Get().mouseType == eMouseAction.Store ||
                this.orderService.mouseAction.Get().mouseType == eMouseAction.Cancel)
            {
                
                Vector3Int dragInitiationLocation = this.environmentService.LocalToCell(new Vector3(dragEvent.initialDragLocation.x + IEnvironmentService.TILE_WIDTH_PIXELS /2, dragEvent.initialDragLocation.y  + IEnvironmentService.TILE_WIDTH_PIXELS /2, 0));
                Vector3Int currentMouseCell = this.environmentService.LocalToCell(new Vector3(dragEvent.currentDragLocation.x  + IEnvironmentService.TILE_WIDTH_PIXELS /2, dragEvent.currentDragLocation.y  + IEnvironmentService.TILE_WIDTH_PIXELS /2, 0));
                if (currentMouseCell != this.lastEnteredCell)
                {
                    IList<Vector3Int> draggedCells = this.environmentService.GetCellsInArea(dragInitiationLocation, currentMouseCell);
                    this.orderSelectionObjects.DestroyAll();
                    draggedCells.ForEach(cell =>
                    {
                        this.orderSelectionObjects.Add(this.orderSelectionFactory.Create(cell, this.environmentService.CellToLocal(cell)));
                    });
                    this.lastEnteredCell = currentMouseCell;
                }
            }
        }

        public override void OnDragEnd(DragEventModel dragEvent)
        {
            this.lastEnteredCell = default(Vector3Int);
            this.orderSelectionObjects.DestroyAll();
        }

        private void RecalculateOrders(IList<UnitOrderModel> _newOrderList)
        {
            IList<UnitOrderModel> newOrders = _newOrderList.Filter(newOrder => { return this.unitOrders.Find(existingOrder => { return existingOrder.ID == newOrder.ID; }) == null; });
            IList<UnitOrderModel> removedOrders = this.unitOrders.Filter(existingOrder => { return _newOrderList.Find(newOrder => { return existingOrder.ID == newOrder.ID; }) == null; });

            newOrders.ForEach(order =>
            {
                this.unitOrders.Add(order);
                this.orderIcons.Add(this.orderIconFactory.Create(order));
            });

            removedOrders.ForEach(order =>
            {
                this.unitOrders = this.unitOrders.Filter(oldOrder => { return oldOrder.ID != order.ID; });
                OrderIcon iconToRemove = this.orderIcons.Find(icon => { return icon.unitOrder.ID == order.ID; });
                this.orderIcons.Remove(iconToRemove);
                iconToRemove.Destroy();
            });
        }

    }
}