using System;
using System.Collections.Generic;
using Building.Models;
using UtilityClasses;
using UI.Models;
using GameControllers.Models;
using Item.Models;
using UnityEngine;

namespace Building
{
    public class GrowerBuildingObject : BuildingObject
    {
        public GrowerBuildingModel growerBuildingModel;
        protected override void OnCreation()
        {
            this.growerBuildingModel = this.buildingObjectModel as GrowerBuildingModel;
        }

        // public void Update()
        // {

        // }

        // public void FixedUpdate()
        // {
            
        // }

        public override void OnSelect()
        {
            IList<BasePanelModel> panels = new List<BasePanelModel>();
            panels.Add(new ObjectPanelModel(this.buildingObjectModel.ID, this.buildingObjectModel.buildingType.ToString(), this.growerBuildingModel));
            panels.Add(new SeedSelectorPanelModel(this.buildingObjectModel.ID, "Select Seed", this.growerBuildingModel));
            this.uiPanelService.selectedObjectPanels.Set(panels);
        }

        // protected override List<string> GenerateContextWindowBody()
        // {
        //     List<string> newContext = base.GenerateContextWindowBody();
        //     newContext.Add("Produces other items");
        //     this.productionBuildingModel.selectedItemRecipe.inputs.ForEach(input =>
        //     {
        //         ItemObjectModel supplyCurrent = this.productionBuildingModel.buildingStorage.GetItem(input.itemType);
        //         newContext.Add(input.itemType.ToString() + ": " +
        //             (supplyCurrent != null ? supplyCurrent.mass : 0).ToString() + "/" +
        //             LocalisationDict.GetMassString(input.mass));
        //     });
        //     return newContext;
        // }
    }
}
