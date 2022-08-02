using System.Collections;
using System.Collections.Generic;
using GameControllers.Services;
using Building.Models;
using UI.Services;
using UtilityClasses;
using UI.Models;

namespace Building
{
    public class StorageBuildingObject : BuildingObject
    {
        public StorageBuildingModel storageBuildingModel;
        public override void Initialise(IContextWindowService _contextService, 
                                        BuildingObjectModel _buildingObjectModel,
                                        IEnvironmentService _environmentService,
                                        IUnitOrderService _orderService)
        {
            base.Initialise(_contextService, _buildingObjectModel, _environmentService, _orderService);
            this.storageBuildingModel = _buildingObjectModel as StorageBuildingModel;
        }

        protected override List<string> GenerateContextWindowBody()
        {
            List<string> newContext = base.GenerateContextWindowBody();
            newContext.Add("Can store other items");
            newContext.Add(((int)this.storageBuildingModel.storageCurrent).ToString() + " " + LocalisationDict.mass);
            return newContext;
        }
    }
}
