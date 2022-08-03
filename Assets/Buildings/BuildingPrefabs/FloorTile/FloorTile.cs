using Building.Models;
using UI.Services;
using GameControllers.Services;
using Item.Models;

namespace Building
{
    public class FloorTile : BuildingObject
    {
        public FloorTileModel floorTileModel;
        protected override void OnCreation()
        {
            this.floorTileModel = this.buildingObjectModel as FloorTileModel;
        }
    }
}