using System;
using System.Collections.Generic;
using UnityEngine;
using Building.Models;

namespace GameControllers.Services
{
    public interface IBuildingService
    {
        public BuildingAssetController buildingAssetController { get; set; }

        public void SetBuildingAssetController(BuildingAssetController _buildingAssetController);
        public SpriteRenderer GetBuildingSprite(eBuildingType buildingType);
    }
}
