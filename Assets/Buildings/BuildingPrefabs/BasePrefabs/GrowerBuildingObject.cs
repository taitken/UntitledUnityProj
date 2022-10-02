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
        private CropObjectModel currentCrop { get { return this.growerBuildingModel.cropObject; } }
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
                CropStatsModel cropStats = this.cropService.GetCropStats((eCropType)this.growerBuildingModel.selectedCropType);
                if (this.growerBuildingModel.GetObjectComponent<ObjectStorageComponent>().GetItem(cropStats.seedItemType) == null)
                {
                    SupplyOrderModel existingOrder = this.unitOrderService.GetOrders<SupplyOrderModel>().Find(order => { return order.objectToSupply == this.growerBuildingModel; });
                    if (existingOrder != null) this.unitOrderService.RemoveOrder(existingOrder.ID);
                    this.unitOrderService.AddOrder(new SupplyOrderModel(this.growerBuildingModel, cropStats.seedItemType, 1));
                }
                else
                {
                    this.growerBuildingModel.cropObject = new CropObjectModel(this.growerBuildingModel.position, new ItemObjectMass(eItemType.OrganicMass, 1), cropStats);;
                    this.cropService.AddCrop(this.growerBuildingModel.cropObject);
                }
            }
            // Replace Crop with another
            if (this.growerBuildingModel.selectedCropType != null && this.currentCrop != null && this.growerBuildingModel.selectedCropType != this.currentCrop.cropType)
            {
                this.unitOrderService.AddOrder(new CropRemoveOrderModel(this.growerBuildingModel.position, this.growerBuildingModel));
            }
        }

        public void ListenForItemsStored(IList<ItemObjectModel> items)
        {
            if (this.currentCrop == null && items.Any(seed => { return this.cropService.GetCropStats((eCropType)this.growerBuildingModel.selectedCropType).seedItemType == seed.itemType; }))
            {
                this.unitOrderService.AddOrder(new CropPlantOrderModel(this.growerBuildingModel.position, (eCropType)this.growerBuildingModel.selectedCropType, this.growerBuildingModel));
            }
        }
    }
}
