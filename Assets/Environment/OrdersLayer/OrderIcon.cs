using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using GameControllers.Services;
using GameControllers.Models;
using Building.Models;
using Zenject;
using UtilityClasses;

namespace Environment
{
    public class OrderIcon : MonoBehaviour2
    {
        private IUnitOrderService orderService;
        private IBuildingService buildingService;
        private MouseActionModel mouseAction;
        private SpriteRenderer spriteRenderer;
        public UnitOrderModel unitOrder;
        public Sprite[] spriteList;
        [Inject]
        public void Construct(IUnitOrderService _orderService,
                                UnitOrderModel _order,
                                IEnvironmentService _envService,
                                IBuildingService _buildingService)
        {
            this.transform.position = _envService.CellToLocal(_order.coordinates);
            this.orderService = _orderService;
            this.buildingService = _buildingService;
            this.unitOrder = _order;
            this.subscriptions.Add(this.orderService.mouseAction.Subscribe(action => { this.mouseAction = action; }));
            
            this.subscriptions.Add(this.orderService.hideOrderIconTrigger.SubscribeQuietly((order =>{
                if(this.unitOrder.ID == order.ID) this.GetComponent<SpriteRenderer>().enabled = false;
            })));
        }
        void Awake()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            if(!this.unitOrder.displayIcon) this.spriteRenderer.enabled = false;
            if (this.unitOrder is SupplyOrderModel)
            {
                SupplyOrderModel buildOrder = this.unitOrder as SupplyOrderModel;
                this.UpdateBuildingSprite(buildOrder.buildingType);
            }
            else
            {
                this.UpdateSprite((int)this.unitOrder.orderType);
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void UpdateSprite(int spriteID)
        {
            this.spriteRenderer.sprite = this.spriteList[spriteID];
        }

        public void UpdateBuildingSprite(eBuildingType buildType)
        {
            this.spriteRenderer.sprite = this.buildingService.GetBuildingSprite(buildType).sprite;
            this.spriteRenderer.color =  GameColors.AddTransparency(this.spriteRenderer.color, 0.6f);
        }

        public override void OnClickedByUser()
        {
            if (this.mouseAction.mouseType == eMouseAction.Cancel)
            {
                this.orderService.RemoveOrder(this.unitOrder.ID);
            }
        }

        public class Factory : PlaceholderFactory<UnitOrderModel, OrderIcon>
        {
        }
    }
}