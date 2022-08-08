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
    public class MineableBlock : MonoBehaviour2
    {
        private IUnitOrderService orderService;
        private IItemObjectService itemService;
        private IContextWindowService contextService;
        private IEnvironmentService environmentService;
        public Sprite[] spriteList;
        private SpriteRenderer spriteRenderer;
        private MouseActionModel mouseAction;
        public MineableObjectModel mineableObjectModel;

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                                IItemObjectService _itemService,
                                IContextWindowService _contextService,
                                IEnvironmentService _environmentService,
                                MineableObjectModel _mineableObjectModel)
        {
            this.environmentService = _environmentService;
            this.mineableObjectModel = _mineableObjectModel;
            this.orderService = _orderService;
            this.itemService = _itemService;
            this.contextService = _contextService;
            this.orderService.mouseAction.Subscribe(this, action => { this.mouseAction = action; });
            this.spriteList = this.environmentService.GetMineableBlockSprites(this.mineableObjectModel.mineableBlockType);
        }
        private void Awake()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }
        // Start is called before the first frame update
        private void Start()
        {
        }

        public override void OnMouseEnter()
        {
            List<string> newContext = new List<string>();
            newContext.Add(this.mineableObjectModel.mass.ToString() + " " + LocalisationDict.mass);
            newContext.Add("Mineable");
            this.contextService.AddContext(new ObjectContextWindowModel(this.mineableObjectModel.ID, this.mineableObjectModel.blockName, newContext));
        }

        public override void OnMouseExit()
        {
            this.contextService.RemoveContext(this.mineableObjectModel.ID);
        }

        protected override void BeforeDeath()
        {
            this.contextService.RemoveContext(this.mineableObjectModel.ID);
            this.itemService.AddItem(new ItemObjectModel(this.mineableObjectModel.position, mineableObjectModel.mass, this.mineableObjectModel.itemDrop, ItemObjectModel.eItemState.OnGround));
        }

        public override void OnClickedByUser()
        {
            if (this.mouseAction.mouseType == eMouseAction.Dig)
            {
                this.orderService.AddOrder(new DigOrderModel(this.mineableObjectModel.position));
            }
        }

        public void updateSprite(int spriteID)
        {
            this.spriteRenderer.sprite = this.spriteList[spriteID];
        }

        public class Factory : PlaceholderFactory<MineableObjectModel, MineableBlock>
        {
        }
    }
}