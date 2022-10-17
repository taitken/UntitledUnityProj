using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Item.Models;
using Zenject;
using UI.Services;
using UI.Models;
using UtilityClasses;
using System;

namespace Item
{
    public class ItemObject : MonoBaseObject
    {
        public GameObject itemSprite;
        public GameObject itemShadow;
        public ItemObjectModel itemObjectModel;
        public IItemObjectService itemService;
        public IEnvironmentService envService;
        public IUnitOrderService orderService;
        private MouseActionModel mouseAction;
        private IList<Vector3> movePath;
        private Vector3 baseShadowLocalScale;
        [Inject]
        public void Construct(ItemObjectModel _itemObjectModel,
                                IUnitOrderService _orderService,
                                IItemObjectService _itemService,
                                IEnvironmentService _envService)
        {
            this.itemObjectModel = _itemObjectModel;
            this.itemService = _itemService;
            this.orderService = _orderService;
            this.envService = _envService;
            this.orderService.mouseAction.Subscribe(this, action => { this.mouseAction = action; });
            this.itemSprite.GetComponent<SpriteRenderer>().sprite = this.itemService.GetItemSprite(this.itemObjectModel.itemType);
            this.baseShadowLocalScale = this.itemShadow.transform.localScale;
        }

        public void Start()
        {
            if (this.itemObjectModel.moveOnSpawn)
            {
                this.BounceSpawn();
                this.itemObjectModel.moveOnSpawn = false;
            }
        }

        public void BounceSpawn()
        {
            this.movePath = new List<Vector3>();
            this.movePath.Add(new Vector3(0, this.transform.position.y + this.itemSprite.transform.localPosition.y + 0.05f));
            this.movePath.Add(new Vector3(0, this.transform.position.y + this.itemSprite.transform.localPosition.y));
            this.movePath.Add(new Vector3(0, this.transform.position.y + this.itemSprite.transform.localPosition.y + 0.025f));
            this.movePath.Add(new Vector3(0, this.transform.position.y + this.itemSprite.transform.localPosition.y));
        }

        public void FixedUpdate()
        {
            if (this.movePath != null && this.movePath.Count > 0)
            {
                MovementHelper.MoveRigidBody2D(this.itemSprite.GetComponent<Rigidbody2D>(), new Vector2(1, 1), 0.2f, this.movePath, this.envService, false);
                this.itemShadow.transform.localScale = this.baseShadowLocalScale * (1 / (1 + this.itemSprite.transform.localPosition.y));
            }
        }

        public override void OnSelect()
        {
            IList<BasePanelModel> panels = new List<BasePanelModel>();
            panels.Add(new ObjectPanelModel(this.itemObjectModel.ID, this.itemObjectModel.itemType.ToString(), this.itemObjectModel));
            this.uiPanelService.selectedObjectPanels.Set(panels);
        }

        public override void OnMouseEnter()
        {
            List<string> newContext = new List<string>();
            newContext.Add(LocalisationDict.GetMassString(this.itemObjectModel.mass));
            newContext.Add("Item");
            this.uiPanelService.AddContext(new ObjectContextWindowModel(this.itemObjectModel.ID, this.itemObjectModel.itemType.ToString(), newContext));
        }

        public override void OnMouseExit()
        {
            this.uiPanelService.RemoveContext(this.itemObjectModel.ID);
        }

        public override void OnClickedByUser()
        {
            if (this.mouseAction.mouseType == eMouseAction.Store)
            {
                this.orderService.AddOrder(new StoreOrderModel(this.itemObjectModel.position, this.itemObjectModel));
            }
        }

        public override BaseObjectModel GetBaseObjectModel()
        {
            return this.itemObjectModel;
        }

        protected override void BeforeDeath()
        {
            this.uiPanelService.RemoveContext(this.itemObjectModel.ID);
        }


        public class Factory : PlaceholderFactory<ItemObjectModel, ItemObject>
        {
        }
    }
}
