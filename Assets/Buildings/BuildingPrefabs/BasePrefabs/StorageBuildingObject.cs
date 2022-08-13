using System.Collections.Generic;
using Building.Models;
using UtilityClasses;

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
            newContext.Add(LocalisationDict.GetMassString(this.storageBuildingModel.storageCurrent));
            return newContext;
        }
    }
}
