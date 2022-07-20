using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Environment.Models;
using Zenject;
using UtilityClasses;
using Item.Models;
using UI.Services;
using UI.Models;

namespace Environment
{
    public class MineableHunk : MonoBehaviour2
    {
        private IUnitOrderService orderService;
        private IItemObjectService itemService;
        private IContextWindowService contextService;
        public Sprite[] spriteList;
        private SpriteRenderer spriteRenderer;
        private MouseActionModel mouseAction;
        public MineableObjectModel mineableObjectModel;

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                                IItemObjectService _itemService,
                                IContextWindowService _contextService,
                                MineableObjectModel _mineableObjectModel)
        {
            this.mineableObjectModel = _mineableObjectModel;
            this.orderService = _orderService;
            this.itemService = _itemService;
            this.contextService = _contextService;
            this.subscriptions.Add(this.orderService.mouseAction.Subscribe(action => { this.mouseAction = action; }));
        }
        private void Awake()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }
        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {

        }

        public override void OnMouseEnter()
        {
            List<string> newContext = new List<string>();
            newContext.Add(this.mineableObjectModel.mass.ToString() + " " + LocalisationDict.mass);
            newContext.Add("Mineable");
            this.contextService.AddContext(new ContextWindowModel(this.mineableObjectModel.ID, "Dirt Block", newContext));
        }

        public override void OnMouseExit()
        {
            this.contextService.RemoveContext(this.mineableObjectModel.ID);
        }

        protected override void BeforeDeath()
        {
            this.contextService.RemoveContext(this.mineableObjectModel.ID);
            this.itemService.AddItem(new ItemObjectModel(this.mineableObjectModel.position, mineableObjectModel.mass, eItemType.Stone, ItemObjectModel.eItemState.OnGround));
        }

        public override void OnClickedByUser()
        {
            if (this.mouseAction.mouseType == eMouseAction.Dig)
            {
                this.orderService.AddOrder(new UnitOrderModel(this.mineableObjectModel.position, eOrderTypes.Dig));
            }
        }

        public void updateSprite(int spriteID)
        {
            this.spriteRenderer.sprite = this.spriteList[spriteID];
        }

        public class Factory : PlaceholderFactory<MineableObjectModel, MineableHunk>
        {
        }
    }
}