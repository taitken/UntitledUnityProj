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
        private IContextWindowService contextService;
        [Inject]
        public void Construct(ItemObjectModel _itemObjectModel,
                                IContextWindowService _contextWindowService)
        {
            this.itemObjectModel = _itemObjectModel;
            this.contextService = _contextWindowService;
        }

        public override void OnMouseEnter()
        {
            List<string> newContext = new List<string>();
            newContext.Add(this.itemObjectModel.mass.ToString() + " " + LocalisationDict.weight);
            newContext.Add("Item");
            this.contextService.AddContext(new ContextWindowModel(this.itemObjectModel.ID, "Stone", newContext));
        }

        public override void OnMouseExit()
        {
            this.contextService.RemoveContext(this.itemObjectModel.ID);
        }

        public class Factory : PlaceholderFactory<ItemObjectModel, ItemObject>
        {
        }
    }
}
