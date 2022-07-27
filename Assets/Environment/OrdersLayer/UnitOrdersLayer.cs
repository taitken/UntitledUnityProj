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
            this.subscriptions.Add(this.orderService.orders.Subscribe(orders =>
            {
                this.RecalculateOrders(orders);
            }));
        }

        public override void OnDrag(DragEventModel dragEvent)
        {
            if (this.orderService.mouseAction.Get().mouseType == eMouseAction.Dig ||
                this.orderService.mouseAction.Get().mouseType == eMouseAction.Store ||
                this.orderService.mouseAction.Get().mouseType == eMouseAction.Cancel)
            {
                Vector3Int dragInitiationLocation = this.environmentService.LocalToCell(dragEvent.initialDragLocation);
                Vector3Int currentMouseCell = this.environmentService.LocalToCell(dragEvent.currentDragLocation);
                if (currentMouseCell != this.lastEnteredCell)
                {
                    List<Vector3Int> draggedCells = new List<Vector3Int>();
                    int xDistance = currentMouseCell.x - dragInitiationLocation.x;
                    int yDistance = currentMouseCell.y - dragInitiationLocation.y;
                    int xSign = Math.Sign(xDistance);
                    int ySign = Math.Sign(yDistance);
                    for (int x = 0; x <= Math.Abs(xDistance); x++)
                    {
                        for (int y = 0; y <= Math.Abs(yDistance); y++)
                        {
                            draggedCells.Add(new Vector3Int(dragInitiationLocation.x + (x * xSign), dragInitiationLocation.y + (y * ySign)));
                        }
                    }
                    this.DestroyAllSelections();
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
            this.DestroyAllSelections();
        }

        private void DestroyAllSelections()
        {
            for (int i = this.orderSelectionObjects.Count - 1; i >= 0; i--)
            {
                this.orderSelectionObjects[i].Destroy();
                this.orderSelectionObjects.RemoveAt(i);
            }
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