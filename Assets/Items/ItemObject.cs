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
        public ItemObjectModel itemObjectModel;
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
            this.contextService = _contextWindowService;
            this.orderService = _orderService;
            this.subscriptions.Add(this.orderService.mouseAction.Subscribe(action => { this.mouseAction = action; }));
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
                this.orderService.AddOrder(new UnitOrderModel(this.itemObjectModel.position, this.mouseAction.mouseType));
            }
        }


        public class Factory : PlaceholderFactory<ItemObjectModel, ItemObject>
        {
        }
    }
}
