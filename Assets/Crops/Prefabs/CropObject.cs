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
using Crops.Models;

namespace Crops
{
    public class CropObject : MonoBaseObject
    {
        public CropObjectModel cropObjectModel;
        public IItemObjectService itemService;
        public IUnitOrderService orderService;
        private IUiPanelService uiPanelService;
        private MouseActionModel mouseAction;
        [Inject]
        public void Construct(CropObjectModel _cropObjectModel,
                                IUiPanelService _contextWindowService,
                                IUnitOrderService _orderService,
                                IItemObjectService _itemService)
        {
            this.cropObjectModel = _cropObjectModel;
            this.itemService = _itemService;
            this.uiPanelService = _contextWindowService;
            this.orderService = _orderService;
            this.orderService.mouseAction.Subscribe(this, action => { this.mouseAction = action; });
        }

        public override void OnSelect()
        {
            IList<BasePanelModel> panels = new List<BasePanelModel>();
            panels.Add(new ObjectPanelModel(this.cropObjectModel.ID, "Crop", this.cropObjectModel));
            this.uiPanelService.selectedObjectPanels.Set(panels);
        }

        public override void OnMouseEnter()
        {
            List<string> newContext = new List<string>();
            newContext.Add(LocalisationDict.GetMassString(this.cropObjectModel.mass));
            newContext.Add("Item");
            this.uiPanelService.AddContext(new ObjectContextWindowModel(this.cropObjectModel.ID, "Crop", newContext));
        }

        public override void OnMouseExit()
        {
            this.uiPanelService.RemoveContext(this.cropObjectModel.ID);
        }

        public override void OnClickedByUser()
        {

        }

        public override BaseObjectModel GetBaseObjectModel()
        {
            return this.cropObjectModel;
        }

        protected override void BeforeDeath()
        {
            this.uiPanelService.RemoveContext(this.cropObjectModel.ID);
        }


        public class Factory : PlaceholderFactory<CropObjectModel, CropObject>
        {
        }
    }
}
