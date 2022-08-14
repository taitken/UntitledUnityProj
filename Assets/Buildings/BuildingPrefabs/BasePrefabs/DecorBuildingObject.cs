using System.Collections.Generic;
using Building.Models;
using UtilityClasses;

namespace Building
{
    public class DecorBuildingObject : BuildingObject
    {
        public DecorBuildingModel decorBuildingModel;

        protected override void OnCreation()
        {
            this.decorBuildingModel = this.buildingObjectModel as DecorBuildingModel;
        }

        protected override List<string> GenerateContextWindowBody()
        {
            List<string> newContext = base.GenerateContextWindowBody();
            newContext.Add("Decorative");
            return newContext;
        }
    }
}
