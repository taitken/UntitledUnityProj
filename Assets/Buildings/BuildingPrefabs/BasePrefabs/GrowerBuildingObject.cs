using System;
using System.Collections.Generic;
using Building.Models;
using UtilityClasses;
using UI.Models;
using GameControllers.Models;
using Item.Models;
using UnityEngine;
using Crops.Models;

namespace Building
{
    public class GrowerBuildingObject : BuildingObject
    {
        public GrowerBuildingModel growerBuildingModel;
        private eCropType? selectedCropType;
        public void Start()
        {
            Debug.Log("ongrowcreation");
            this.growerBuildingModel = this.buildingObjectModel as GrowerBuildingModel;
            this.growerBuildingModel.ListenForUpdates(this.ListenForModelUpdates);
            this.selectedCropType = this.growerBuildingModel.selectedCropType;
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
            if (this.growerBuildingModel.selectedCropType != null && this.selectedCropType == null)
            {
                this.unitOrderService.AddOrder(new SupplyOrderModel(this.growerBuildingModel, this.cropService.GetCropStats((eCropType)this.growerBuildingModel.selectedCropType).seedItemType, 1));
            }
        }
    }
}
