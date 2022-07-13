using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using UnityEngine.Tilemaps;
using Building.Models;

namespace GameControllers.Services
{
    public class BuildingService : IBuildingService
    {
        public BuildingAssetController buildingAssetController { get; set; }

        public void SetBuildingAssetController(BuildingAssetController _buildingAssetController)
        {
            this.buildingAssetController = _buildingAssetController;
        }

        public SpriteRenderer GetBuildingSprite(eBuildingType buildingType)
        {
            return this.buildingAssetController.GetBuildingSprite(buildingType);
        }
    }
}