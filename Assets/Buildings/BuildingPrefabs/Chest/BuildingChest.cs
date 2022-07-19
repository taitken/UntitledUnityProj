using Building.Models;
using UI.Services;
using GameControllers.Services;

namespace Building
{
    public class BuildingChest : StorageBuildingObject
    {
        public ChestBuildingModel chestBuildingModel;
        public override void Initialise(IContextWindowService _contextService, BuildingObjectModel _buildingObjectModel, IEnvironmentService _environmentService)
        {
            base.Initialise(_contextService, _buildingObjectModel, _environmentService);
            this.chestBuildingModel = _buildingObjectModel as ChestBuildingModel;
        }
    }
}