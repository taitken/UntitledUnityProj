using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Building.Models;
using Zenject;
using UnityEngine.Tilemaps;
using UI.Services;
using UtilityClasses;
using UI.Models;

namespace Building
{
    public class StorageBuildingObject : BuildingObject
    {
        public StorageBuildingModel storageBuildingModel;
        public override void Initialise(IContextWindowService _contextService, BuildingObjectModel _buildingObjectModel, Tilemap tilemap)
        {
            base.Initialise(_contextService, _buildingObjectModel, tilemap);
            this.storageBuildingModel = _buildingObjectModel as StorageBuildingModel;
        }

        protected override List<string> GenerateContextWindowBody()
        {
            List<string> newContext = base.GenerateContextWindowBody();
            newContext.Add("Can store other items");
            newContext.Add(((int)this.storageBuildingModel.storageMax).ToString() + " " + LocalisationDict.mass);
            return newContext;
        }
    }
}
