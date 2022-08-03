using System;
using System.Collections.Generic;
using UnityEngine;
using Building.Models;
using UtilityClasses;
using Building;

namespace GameControllers.Services
{
    public interface IBuildingService : IBaseService
    {
        public BuildingAssetController buildingAssetController { get; set; }
        MonoObseravable<IList<BuildingObjectModel>> buildingObseravable { get; set; }
        MonoObseravable<IList<BuildSiteModel>> buildingSiteObseravable { get; set; }
        public void SetBuildingAssetController(BuildingAssetController _buildingAssetController);
        public GameObject GetBuildingGhostPrefab(eBuildingType buildingType);
        public BuildingObject GetBuildingPrefab(eBuildingType buildingType);
        public SpriteRenderer GetBuildingSprite(eBuildingType buildingType);
        void AddBuilding(BuildingObjectModel building);
        void RemoveBuilding(long id);
        void AddBuildSite(BuildSiteModel buildSite);
        void RemoveBuildSite(long id);
        BuildingObjectModel GetClosestStorage(Vector3Int startPos);
        bool IsStorageAvailable();
        bool IsBuildingSpaceAvailable(Vector3Int _location);
        bool IsFloorSpaceAvailable(Vector3Int _location);
    }
}
