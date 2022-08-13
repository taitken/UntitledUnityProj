
using System;
using UnityEngine;
using System.Collections.Generic;
using Environment.Models;
using GameControllers.Models;
using GameControllers.Services;
using Item.Models;
using Unit.Models;
using static Item.Models.ItemObjectModel;

namespace UnitAction
{
    public class CreateNewStoreOrderAction : IUnitAction
    {
        private IUnitOrderService orderService;
        private IItemObjectService itemService;
        private UnitModel unit;
        private Vector3Int coordinates;
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public CreateNewStoreOrderAction(Vector3Int _coordinates,
                          IUnitOrderService _orderService,
                          IItemObjectService _itemService)
        {
            this.orderService = _orderService;
            this.itemService = _itemService;
            this.coordinates = _coordinates;
        }

        public bool CheckCompleted()
        {
            return this.completed;
        }

        public void CancelAction()
        {
            this.cancel = true;
        }
        public bool PerformAction()
        {
            ItemObjectModel itemObj = this.itemService.itemObseravable.Get().Find(item => { return item.itemState == eItemState.OnGround && item.position == this.coordinates; });
            if (itemObj != null)
            {
                this.orderService.AddOrder(new StoreOrderModel(this.coordinates, itemObj));
            }
            this.completed = true;
            return this.completed;
        }
    }
}