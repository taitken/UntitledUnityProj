using Building.Models;
using UI.Services;
using GameControllers.Services;

namespace Building
{
    public class BuildingSmelter : ProductionBuildingObject
    {
        public ProductionBuildingModel smelterBuildingModel;
        public override void Initialise(IContextWindowService _contextService, BuildingObjectModel _buildingObjectModel, IEnvironmentService _environmentService,
                                        IUnitOrderService _orderService)
        {
            base.Initialise(_contextService, _buildingObjectModel, _environmentService, _orderService);
            this.smelterBuildingModel = _buildingObjectModel as ProductionBuildingModel;
        }
    }
}