using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;

namespace Environment
{
    public class UnitOrdersLayer : MonoBehaviourLayer
    {
        private IUnitOrderService orderService;
        private IList<UnitOrderModel> unitOrders;
        private IList<OrderIcon> orderIcons;
        private OrderIcon.Factory orderIconFactory;

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              OrderIcon.Factory _orderIconFactory,
                              LayerCollider.Factory _layerColliderFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "UnitOrderLayer");
            this.orderIconFactory = _orderIconFactory;
            this.orderIcons = new List<OrderIcon>();
            this.unitOrders = new List<UnitOrderModel>();
            this.orderService = _orderService;
            this.subscriptions.Add(this.orderService.orders.Subscribe(orders =>
            {
                this.RecalculateOrders(orders);
            }));
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