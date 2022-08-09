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

namespace Item
{
    public class ItemObject : MonoBasePhysicalObject
    {
        public ItemObjectModel itemObjectModel;
        public IItemObjectService itemService;
        public IUnitOrderService orderService;
        private IUiPanelService uiPanelService;
        private MouseActionModel mouseAction;
        [Inject]
        public void Construct(ItemObjectModel _itemObjectModel,
                                IUiPanelService _contextWindowService,
                                IUnitOrderService _orderService,
                                IItemObjectService _itemService)
        {
            this.itemObjectModel = _itemObjectModel;
            this.itemService = _itemService;
            this.uiPanelService = _contextWindowService;
            this.orderService = _orderService;
            this.orderService.mouseAction.Subscribe(this, action => { this.mouseAction = action; });
            this.GetComponent<SpriteRenderer>().sprite = this.itemService.GetItemSprite(this.itemObjectModel.itemType);
        }

        public override void OnSelect()
        {
            this.uiPanelService.selectedObject.Set(new PanelModel(this.itemObjectModel.ID, this.itemObjectModel.itemType.ToString(), ePanelTypes.ObjectInfo));
        }

        public override void OnMouseEnter()
        {
            List<string> newContext = new List<string>();
            newContext.Add(this.itemObjectModel.mass.ToString() + " " + LocalisationDict.mass);
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

        protected override void BeforeDeath()
        {
            this.uiPanelService.RemoveContext(this.itemObjectModel.ID);
        }


        public class Factory : PlaceholderFactory<ItemObjectModel, ItemObject>
        {
        }
    }
}
