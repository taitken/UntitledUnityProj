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
    public class ItemObject : MonoBehaviour2
    {
        public List<Sprite> itemSprites;
        public ItemObjectModel itemObjectModel;
        public IItemObjectService itemService;
        public IUnitOrderService orderService;
        private IContextWindowService contextService;
        private MouseActionModel mouseAction;
        [Inject]
        public void Construct(ItemObjectModel _itemObjectModel,
                                IContextWindowService _contextWindowService,
                                IUnitOrderService _orderService,
                                IItemObjectService _itemService)
        {
            this.itemObjectModel = _itemObjectModel;
            this.itemService = _itemService;
            this.contextService = _contextWindowService;
            this.orderService = _orderService;
            this.subscriptions.Add(this.orderService.mouseAction.Subscribe(action => { this.mouseAction = action; }));
            this.GetComponent<SpriteRenderer>().sprite = this.itemSprites[(int)this.itemObjectModel.itemType];
        }

        public override void OnMouseEnter()
        {
            List<string> newContext = new List<string>();
            newContext.Add(this.itemObjectModel.mass.ToString() + " " + LocalisationDict.mass);
            newContext.Add("Item");
            this.contextService.AddContext(new ContextWindowModel(this.itemObjectModel.ID, "Stone", newContext));
        }

        public override void OnMouseExit()
        {
            this.contextService.RemoveContext(this.itemObjectModel.ID);
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
            this.contextService.RemoveContext(this.itemObjectModel.ID);
        }


        public class Factory : PlaceholderFactory<ItemObjectModel, ItemObject>
        {
        }
    }
}
