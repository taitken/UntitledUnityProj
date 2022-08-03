using System.Collections;
using System.Collections.Generic;
using GameControllers.Services;
using Building.Models;
using UI.Services;
using UtilityClasses;
using UI.Models;

namespace Building
{
    public class StorageBuildingObject : BuildingObject
    {
        public StorageBuildingModel storageBuildingModel;

        protected override void OnCreation()
        {
            this.storageBuildingModel = this.buildingObjectModel as StorageBuildingModel;
        }

        protected override List<string> GenerateContextWindowBody()
        {
            List<string> newContext = base.GenerateContextWindowBody();
            newContext.Add("Can store other items");
            newContext.Add(((int)this.storageBuildingModel.storageCurrent).ToString() + " " + LocalisationDict.mass);
            return newContext;
        }
    }
}
