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
        public Subscribable<IList<BuildingObjectModel>> buildingSubscribable { get; set; } = new Subscribable<IList<BuildingObjectModel>>(new List<BuildingObjectModel>());

        public void SetBuildingAssetController(BuildingAssetController _buildingAssetController)
        {
            this.buildingAssetController = _buildingAssetController;
        }

        public SpriteRenderer GetBuildingSprite(eBuildingType buildingType)
        {
            return this.buildingAssetController.GetBuildingSprite(buildingType);
        }
        public void AddBuilding(BuildingObjectModel building)
        {
            IList<BuildingObjectModel> _buildings = this.buildingSubscribable.Get();
            if (_buildings.Find(existingUnit => { return building.ID == existingUnit.ID; }) == null)
            {
                _buildings.Add(building);
                this.buildingSubscribable.Set(_buildings);
            }
        }
        public void RemoveBuilding(long id)
        {
            this.buildingSubscribable.Set(this.buildingSubscribable.Get().Filter(building => { return building.ID != id; }));
        }
    }
}