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
using ObjectComponents;
using Building.Models;

namespace Crops
{

    public class CropObject : MonoBaseObject
    {
        public CropObjectModel cropObjectModel;
        public IItemObjectService itemService;
        public IUnitOrderService orderService;
        private IBuildingService buildingService;
        private ICropService cropService;
        private IDayCycleService dayCycleService;
        private MouseActionModel mouseAction;
        [Inject]
        public void Construct(CropObjectModel _cropObjectModel,
                                ICropService _cropService,
                                IUnitOrderService _orderService,
                                IItemObjectService _itemService,
                                IBuildingService _buildingService,
                                IDayCycleService _dayCycleService)
        {
            this.cropObjectModel = _cropObjectModel;
            this.itemService = _itemService;
            this.cropService = _cropService;
            this.dayCycleService = _dayCycleService;
            this.orderService = _orderService;
            this.buildingService = _buildingService;
            this.orderService.mouseAction.Subscribe(this, action => { this.mouseAction = action; });
            this.dayCycleService.OnHourTickObservable.SubscribeQuietly(this, this.AddGrowTick);
            this.GetComponent<SpriteRenderer>().sprite = this.cropService.GetCropSpriteSet(this.cropObjectModel.cropType)[0];
        }

        private void AddGrowTick(int hour)
        {
            this.cropObjectModel.growTicks++;
            this.UpdateCropStage();
        }

        public override void OnSelect()
        {
            IList<BasePanelModel> panels = new List<BasePanelModel>();
            panels.Add(new ObjectPanelModel(this.cropObjectModel.ID, this.cropObjectModel.cropName, this.cropObjectModel));
            this.uiPanelService.selectedObjectPanels.Set(panels);
        }

        private void UpdateCropStage()
        {
            float tickDivider = 1f / (float)CropObjectModel.COMPLETED_GROW_STAGE;
            this.cropObjectModel.growStage = Math.Min((int)CropObjectModel.NUM_GROW_STAGES, (int)((float)this.cropObjectModel.growTicks / (float)this.cropObjectModel.growTime / tickDivider) + 1);
            this.GetComponent<SpriteRenderer>().sprite = this.cropService.GetCropSpriteSet(this.cropObjectModel.cropType)[(int)this.cropObjectModel.growStage - 1];
            if (this.cropObjectModel.growStage >= CropObjectModel.COMPLETED_GROW_STAGE)
            {
                this.AddFruit();
                this.AddHarvestOrder();
            }
        }

        private void AddFruit()
        {
            CropStatsModel cropStats = CropStatsLibrary.GetCropStats(this.cropObjectModel.cropType);
            ObjectCompositionComponent cropCompoisition = this.cropObjectModel.GetObjectComponent<ObjectCompositionComponent>();
            cropStats.producedItems.ForEach(item =>
            {
                if (cropCompoisition.GetMass(item.itemType) == 0)
                {
                    cropCompoisition.AddToComposition(item.itemType, item.mass);
                }
            });
        }

        private void AddHarvestOrder()
        {
            if (!this.orderService.GetOrders<CropHarvestOrderModel>().Any(crop => { return crop.cropObjectModel == this.cropObjectModel; }))
            {
                GrowerBuildingModel building = this.buildingService.buildingObseravable.Get().Map(building =>{return building as GrowerBuildingModel;}).Find(building =>{return building != null && building.cropObject == this.cropObjectModel;});
                this.orderService.AddOrder(new CropHarvestOrderModel(this.cropObjectModel.position, this.cropObjectModel, building));
            }
        }

        public override void OnMouseEnter()
        {
            List<string> newContext = new List<string>();
            newContext.Add(LocalisationDict.GetMassString(this.cropObjectModel.mass));
            newContext.Add("Item");
            this.uiPanelService.AddContext(new ObjectContextWindowModel(this.cropObjectModel.ID, this.cropObjectModel.cropName, newContext));
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
        public class Factory : PlaceholderFactory<CropObjectModel, CropObject>
        {
        }
    }
}
