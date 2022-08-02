using Building.Models;
using UI.Services;
using GameControllers.Services;
using Item.Models;

namespace Building
{
    public class BuildingChest : StorageBuildingObject
    {
        public StorageBuildingModel chestBuildingModel;
        public override void Initialise(IContextWindowService _contextService, BuildingObjectModel _buildingObjectModel, IEnvironmentService _environmentService,
                                        IUnitOrderService _orderService)
        {
            base.Initialise(_contextService, _buildingObjectModel, _environmentService, _orderService);
            this.chestBuildingModel = _buildingObjectModel as StorageBuildingModel;
        }
    }
}