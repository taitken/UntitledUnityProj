using System;
using System.Collections.Generic;
using UnityEngine;
using Building.Models;
using UtilityClasses;

namespace GameControllers.Services
{
    public interface IBuildingService
    {
        public BuildingAssetController buildingAssetController { get; set; }
        Subscribable<IList<BuildingObjectModel>> buildingSubscribable { get; set; }
        public void SetBuildingAssetController(BuildingAssetController _buildingAssetController);
        public SpriteRenderer GetBuildingSprite(eBuildingType buildingType);
        void AddBuilding(BuildingObjectModel building);
        void RemoveBuilding(long id);
    }
}
