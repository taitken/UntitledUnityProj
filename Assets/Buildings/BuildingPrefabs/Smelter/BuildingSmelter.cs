using Building.Models;
using UI.Services;
using GameControllers.Services;

namespace Building
{
    public class BuildingSmelter : ProductionBuildingObject
    {
        public ProductionBuildingModel smelterBuildingModel;
        protected override void OnCreation()
        {
            base.OnCreation();
            this.smelterBuildingModel = this.buildingObjectModel as ProductionBuildingModel;
        }
    }
}