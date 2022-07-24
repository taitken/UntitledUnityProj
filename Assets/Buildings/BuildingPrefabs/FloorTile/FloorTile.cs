using Building.Models;
using UI.Services;
using GameControllers.Services;
using Item.Models;

namespace Building
{
    public class FloorTile : BuildingObject
    {
        public FloorTileModel floorTileModel;
        public override void Initialise(IContextWindowService _contextService, BuildingObjectModel _buildingObjectModel, IEnvironmentService _environmentService)
        {
            base.Initialise(_contextService, _buildingObjectModel, _environmentService);
            this.floorTileModel = _buildingObjectModel as FloorTileModel;
        }
    }
}