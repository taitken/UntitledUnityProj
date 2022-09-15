using System;
using System.Collections.Generic;
using Building.Models;
using UtilityClasses;
using UI.Models;
using GameControllers.Models;
using Item.Models;
using UnityEngine;
using Crops.Models;
using ObjectComponents;

namespace Building
{
    public class GrowerBuildingObject : BuildingObject
    {
        public GrowerBuildingModel growerBuildingModel;
        private CropObjectModel currentCrop;
        public void Start()
        {
            this.growerBuildingModel = this.buildingObjectModel as GrowerBuildingModel;
            this.growerBuildingModel.ListenForUpdates(this.ListenForModelUpdates);
            this.growerBuildingModel.GetObjectComponent<ObjectStorageComponent>().ListenForItemsAdded(this.ListenForItemsStored);
        }

        public override void OnSelect()
        {
            IList<BasePanelModel> panels = new List<BasePanelModel>();
            panels.Add(new ObjectPanelModel(this.buildingObjectModel.ID, this.buildingObjectModel.buildingType.ToString(), this.growerBuildingModel));
            panels.Add(new SeedSelectorPanelModel(this.buildingObjectModel.ID, "Select Seed", this.growerBuildingModel));
            this.uiPanelService.selectedObjectPanels.Set(panels);
        }

        public void ListenForModelUpdates()
        {
            // Selected crop when none is selected
            if (this.growerBuildingModel.selectedCropType != null && this.currentCrop == null)
            {
                this.unitOrderService.AddOrder(new SupplyOrderModel(this.growerBuildingModel, this.cropService.GetCropStats((eCropType)this.growerBuildingModel.selectedCropType).seedItemType, 1));
            }
        }

        public void ListenForItemsStored(IList<ItemObjectModel> items)
        {
            if(this.currentCrop == null && items.Any(seed => {return this.cropService.GetCropStats((eCropType)this.growerBuildingModel.selectedCropType).seedItemType == seed.itemType;}))
            {
                this.unitOrderService.AddOrder(new CropPlantOrderModel(this.growerBuildingModel.position, (eCropType)this.growerBuildingModel.selectedCropType, this.growerBuildingModel));
            }
        }
    }
}
