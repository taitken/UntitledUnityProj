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
        public Obseravable<IList<BuildingObjectModel>> buildingObseravable { get; set; } = new Obseravable<IList<BuildingObjectModel>>(new List<BuildingObjectModel>());
        private IList<StorageBuildingModel> storageBuilding { get { return this.buildingObseravable.Get().Filter(building => { return building is StorageBuildingModel; }).Map(storageBuilding => { return storageBuilding as StorageBuildingModel; }); } }

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
            IList<BuildingObjectModel> _buildings = this.buildingObseravable.Get();
            if (_buildings.Find(existingUnit => { return building.ID == existingUnit.ID; }) == null)
            {
                _buildings.Add(building);
                this.buildingObseravable.Set(_buildings);
            }
        }
        public void RemoveBuilding(long id)
        {
            this.buildingObseravable.Set(this.buildingObseravable.Get().Filter(building => { return building.ID != id; }));
        }
        public Vector3Int GetClosestStorageLocation(Vector3Int startPos)
        {
            long? lowestDistance = null;
            Vector3Int returnVec = default(Vector3Int);
            this.storageBuilding.ForEach(building =>
            {
                long distance = Math.Abs(startPos.x - building.position.x) + Math.Abs(startPos.y - building.position.y);
                if(lowestDistance == null || distance < lowestDistance)
                {
                    lowestDistance = distance;
                    returnVec = building.position;
                }
            });
            return returnVec;
        }
        public bool IsStorageAvailable()
        {
            return this.storageBuilding.Count > 0;
        }

    }
}