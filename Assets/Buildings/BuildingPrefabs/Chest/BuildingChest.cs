using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Building.Models;
using UtilityClasses;
using UI.Models;
using UnityEngine.Tilemaps;
using UI.Services;

namespace Building
{
    public class BuildingChest : StorageBuildingObject
    {
        public ChestBuildingModel chestBuildingModel;
        public override void Initialise(IContextWindowService _contextService, BuildingObjectModel _buildingObjectModel, Tilemap tilemap)
        {
            base.Initialise(_contextService, _buildingObjectModel, tilemap);
            this.chestBuildingModel = _buildingObjectModel as ChestBuildingModel;
        }
    }
}