using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Environment.Models;
using Zenject;

namespace Environment
{
    public class MineableHunk : MonoBehaviour2
    {
        private IUnitOrderService orderService;
        public Sprite[] spriteList;
        private SpriteRenderer spriteRenderer;
        private eMouseAction mouseAction;
        public MineableObjectModel mineableObjectModel;

        [Inject]
        public void Construct(IUnitOrderService _orderService, MineableObjectModel _mineableObjectModel)
        {
            this.mineableObjectModel = _mineableObjectModel;
            this.orderService = _orderService;
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

        void OnMouseDown()
        {
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