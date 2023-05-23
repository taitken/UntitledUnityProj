using System;
using Building.Models;
using UnityEngine;

namespace Building
{
    public class BuildingObjectFactory
    {
        public BuildingObjectFactory()
        {

        }

        public BuildingObjectModel CreateBuildingModel(Vector3Int _position, eBuildingType _buildingType)
        {
            BuildingObjectModel newBuilding;
            BuildingStatsModel buildStats = BuildingStatsLibrary.GetBuildingStats(_buildingType);
            switch (buildStats.buildCategory)
            {
                case eBuildingCategory.Storage:
                    newBuilding = new StorageBuildingModel(_position, _buildingType, buildStats);
                    break;
                case eBuildingCategory.FloorTile:
                    newBuilding = new FloorTileModel(_position, _buildingType, buildStats);
                    break;
                case eBuildingCategory.Production:
                    newBuilding = new ProductionBuildingModel(_position, _buildingType, buildStats);
                    break;
                case eBuildingCategory.Decor:
                    newBuilding = new DecorBuildingModel(_position, _buildingType, buildStats);
                    break;
                case eBuildingCategory.Wall:
                    newBuilding = new WallBuildingModel(_position, _buildingType, buildStats);
                    break;
                case eBuildingCategory.Door:
                    newBuilding = new DoorBuildingModel(_position, _buildingType, buildStats);
                    break;
                case eBuildingCategory.Grower:
                    newBuilding = new GrowerBuildingModel(_position, _buildingType, buildStats);
                    break;
                default:
                    newBuilding = null;
                    Debug.LogException(new System.Exception("Building Model Factory failed to build model as no legitmate building type was supplied"));
                    break;
            }
            return newBuilding;
        }
    }
}