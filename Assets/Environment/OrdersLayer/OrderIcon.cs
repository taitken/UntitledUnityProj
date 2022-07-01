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
        private IUnitActionService actionService;
        private eMouseAction mouseAction;
        private SpriteRenderer spriteRenderer;
        public UnitOrderModel unitOrder;
        public Sprite[] spriteList;
        [Inject]
        public void Construct(IUnitActionService _actionService, UnitOrderModel _order)
        {
            this.transform.position = _order.coordinates;
            this.actionService = _actionService;
            this.unitOrder = _order;
            this.subscriptions.Add(this.actionService.mouseAction.Subscribe(action => { this.mouseAction = action; }));
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
                this.actionService.RemoveOrder(this.unitOrder.ID);
            }
        }

        public class Factory : PlaceholderFactory<UnitOrderModel, OrderIcon>
        {
        }
    }
}