using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using UnityEngine.Tilemaps;
using Building.Models;
using Building;

namespace GameControllers.Services
{
    public class BuildingService : BaseService, IBuildingService
    {
        public BuildingAssetController buildingAssetController { get; set; }
        public MonoObseravable<IList<BuildingObjectModel>> buildingObseravable { get; set; } = new MonoObseravable<IList<BuildingObjectModel>>(new List<BuildingObjectModel>());
        public MonoObseravable<BuildingObjectModel> newBuildingObservable { get; set; } = new MonoObseravable<BuildingObjectModel>(null);
        public MonoObseravable<BuildingObjectModel> removedBuildingObservable { get; set; } = new MonoObseravable<BuildingObjectModel>(null);
        public MonoObseravable<IList<BuildSiteModel>> buildingSiteObseravable { get; set; } = new MonoObseravable<IList<BuildSiteModel>>(new List<BuildSiteModel>());
        private IList<StorageBuildingModel> storageBuilding { get { return this.buildingObseravable.Get().Filter(building => { return building is StorageBuildingModel; }).Map(storageBuilding => { return storageBuilding as StorageBuildingModel; }); } }

        public void SetBuildingAssetController(BuildingAssetController _buildingAssetController)
        {
            this.buildingAssetController = _buildingAssetController;
            this.buildingAssetController.Initialise();
        }

        public GameObject GetBuildingGhostPrefab(eBuildingType buildingType)
        {
            return this.buildingAssetController.GetBuildingGhostPrefab(buildingType);
        }

        public BuildingObject GetBuildingPrefab(eBuildingType buildingType)
        {
            return this.buildingAssetController.GetBuildingPrefab(buildingType);
        }

        public SpriteRenderer GetBuildingSprite(eBuildingType buildingType)
        {
            return this.buildingAssetController.GetBuildingSprite(buildingType);
        }
        public Sprite[] GetWallSprites(eWallTypes wallType)
        {
            return this.buildingAssetController.GetWallSpriteSet(wallType);
        }
        public void AddBuilding(BuildingObjectModel building)
        {
            IList<BuildingObjectModel> _buildings = this.buildingObseravable.Get();
            if (building != null && _buildings.Find(existingUnit => { return building.ID == existingUnit.ID; }) == null)
            {
                _buildings.Add(building);
                this.buildingObseravable.Set(_buildings);
                this.newBuildingObservable.Set(building);
            }
        }

        public IList<BuildingObjectModel> GetBuildings()
        {
            return this.buildingObseravable.Get();
        }

        // Not working for some reason
        public IList<T> GetBuildings<T>() where T : BuildingObjectModel
        {
            return this.buildingObseravable.Get().Filter(building => { return building.buildingType is T; }).Map(building => { return building as T; });
        }
        public void RemoveBuilding(long id)
        {
            BuildingObjectModel removedBuilding = this.buildingObseravable.Get().Find(building => { return building.ID == id; });
            this.buildingObseravable.Set(this.buildingObseravable.Get().Filter(building => { return building.ID != id; }));
            this.removedBuildingObservable.Set(removedBuilding);
        }

        public void SubscribeToNewBuildingTrigger(MonoBehaviour2 monobehaviour, Action<BuildingObjectModel> _newBuilding)
        {
            this.newBuildingObservable.SubscribeQuietly(monobehaviour, _newBuilding);
        }

        public void SubscribeToRemovedBuildingTrigger(MonoBehaviour2 monobehaviour, Action<BuildingObjectModel> _newBuilding)
        {
            this.removedBuildingObservable.SubscribeQuietly(monobehaviour, _newBuilding);
        }

        public void AddBuildSite(BuildSiteModel buildSite)
        {
            IList<BuildSiteModel> _buildSites = this.buildingSiteObseravable.Get();
            if (_buildSites.Find(existingBuildSite => { return buildSite.ID == existingBuildSite.ID; }) == null)
            {
                _buildSites.Add(buildSite);
                this.buildingSiteObseravable.Set(_buildSites);
            }
        }
        public void RemoveBuildSite(long id)
        {
            this.buildingSiteObseravable.Set(this.buildingSiteObseravable.Get().Filter(buildSite => { return buildSite.ID != id; }));
        }
        public BuildingObjectModel GetClosestStorage(Vector3Int startPos)
        {
            long? lowestDistance = null;
            BuildingObjectModel returnModel = null;
            this.storageBuilding.ForEach(building =>
            {
                long distance = Math.Abs(startPos.x - building.position.x) + Math.Abs(startPos.y - building.position.y);
                if (lowestDistance == null || distance < lowestDistance)
                {
                    lowestDistance = distance;
                    returnModel = building;
                }
            });
            return returnModel;
        }
        public bool IsStorageAvailable()
        {
            return this.storageBuilding.Count > 0;
        }

        public bool IsBuildingSpaceAvailable(Vector3Int _location)
        {
            IList<Vector3Int> locations = new List<Vector3Int>();
            this.buildingObseravable.Get().Filter(building => { return building.buildingType != eBuildingType.FloorTile; }).ForEach(building => { building.positions.ForEach(pos => { locations.Add(pos); }); });
            this.buildingSiteObseravable.Get().ForEach(site => { site.buildingModel.positions.ForEach(pos => { locations.Add(site.position); }); });
            return !locations.Any(location => { return location == _location; });
        }

        public bool IsFloorSpaceAvailable(Vector3Int _location)
        {
            IList<Vector3Int> locations = new List<Vector3Int>();
            this.buildingObseravable.Get().Filter(building => { return building.buildingType == eBuildingType.FloorTile; }).ForEach(building => { building.positions.ForEach(pos => { locations.Add(pos); }); });
            this.buildingSiteObseravable.Get().ForEach(site => { site.buildingModel.positions.ForEach(pos => { locations.Add(site.position); }); });
            return !locations.Any(location => { return location == _location; });
        }

    }
}