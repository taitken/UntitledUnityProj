using Building.Models;
using UI.Services;
using GameControllers.Services;
using Item.Models;

namespace Building
{
    public class FarmPlot : GrowerBuildingObject
    {
        protected override void OnCreation()
        {
            this.growerBuildingModel = this.buildingObjectModel as GrowerBuildingModel;
        }
    }
}