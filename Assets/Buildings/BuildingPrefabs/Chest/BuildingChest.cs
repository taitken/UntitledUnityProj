using Building.Models;
using UI.Services;
using GameControllers.Services;
using Item.Models;

namespace Building
{
    public class BuildingChest : StorageBuildingObject
    {
        public StorageBuildingModel chestBuildingModel;

        protected override void OnCreation()
        {
            base.OnCreation();
            this.chestBuildingModel = this.buildingObjectModel as StorageBuildingModel;
        }

    }
}