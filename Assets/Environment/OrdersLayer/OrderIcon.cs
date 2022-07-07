using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;

namespace Environment
{
    public class OrderIcon : MonoBehaviour2
    {
        private IUnitOrderService orderService;
        private eMouseAction mouseAction;
        private SpriteRenderer spriteRenderer;
        public UnitOrderModel unitOrder;
        public Sprite[] spriteList;
        [Inject]
        public void Construct(IUnitOrderService _orderService, UnitOrderModel _order, IEnvironmentService _envService)
        {
            this.transform.position = _envService.CellToLocal(_order.coordinates);
            this.orderService = _orderService;
            this.unitOrder = _order;
            this.subscriptions.Add(this.orderService.mouseAction.Subscribe(action => { this.mouseAction = action; }));
        }
        void Awake()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.updateSprite((int)this.unitOrder.orderType);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void updateSprite(int spriteID)
        {
            this.spriteRenderer.sprite = this.spriteList[spriteID];
        }

        public override void OnClickedByUser()
        {
            if (this.mouseAction == eMouseAction.Cancel)
            {
                this.orderService.RemoveOrder(this.unitOrder.ID);
            }
        }

        public class Factory : PlaceholderFactory<UnitOrderModel, OrderIcon>
        {
        }
    }
}