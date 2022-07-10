using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Environment.Models;
using Zenject;
using Item.Models;

namespace Environment
{
    public class MineableHunk : MonoBehaviour2
    {
        private IUnitOrderService orderService;
        private IItemObjectService itemService;
        public Sprite[] spriteList;
        private SpriteRenderer spriteRenderer;
        private eMouseAction mouseAction;
        public MineableObjectModel mineableObjectModel;

        [Inject]
        public void Construct(IUnitOrderService _orderService, 
                                IItemObjectService _itemService,
                                MineableObjectModel _mineableObjectModel)
        {
            this.mineableObjectModel = _mineableObjectModel;
            this.orderService = _orderService;
            this.itemService = _itemService;
            this.subscriptions.Add(this.orderService.mouseAction.Subscribe(action => { this.mouseAction = action; }));
        }
        void Awake()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void BeforeDeath()
        {
            this.itemService.AddItem(new ItemObjectModel(this.mineableObjectModel.position));
        }



        public override void OnClickedByUser()
        {
            if (this.mouseAction == eMouseAction.Dig)
            {
                this.orderService.AddOrder(new UnitOrderModel(this.mineableObjectModel.position, this.mouseAction));
            }
        }

        public void updateSprite(int spriteID)
        {
            this.spriteRenderer.sprite = this.spriteList[spriteID];
        }

        public class Factory : PlaceholderFactory<MineableObjectModel,  MineableHunk>
        {
        }
    }
}