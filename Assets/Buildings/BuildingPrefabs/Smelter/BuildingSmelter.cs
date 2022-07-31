using Building.Models;
using UI.Services;
using GameControllers.Services;

namespace Building
{
    public class BuildingSmelter : BuildingObject
    {
        public ProductionBuildingModel smelterBuildingModel;
        public override void Initialise(IContextWindowService _contextService, BuildingObjectModel _buildingObjectModel, IEnvironmentService _environmentService)
        {
            base.Initialise(_contextService, _buildingObjectModel, _environmentService);
            this.smelterBuildingModel = _buildingObjectModel as ProductionBuildingModel;
        }
    }
}