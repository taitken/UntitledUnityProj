using System;
using GameControllers.Models;
using GameControllers.Services;
using Building.Models;
using UnityEngine;
using Item.Models;
using System.Collections.Generic;

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
            BuildingStatsModel buildStats = BuildingTypeStats.GetBuildingStats(_buildingType);
            switch (_buildingType)
            {
                case eBuildingType.Chest:
                    newBuilding = new StorageBuildingModel(_position, buildStats.size, _buildingType, buildStats.buildSupply, 10000.00M);
                    break;
                case eBuildingType.FloorTile:
                    newBuilding = new FloorTileModel(_position, buildStats.size, _buildingType, buildStats.buildSupply);
                    break;
                case eBuildingType.Smelter:
                    newBuilding = new ProductionBuildingModel(_position, buildStats.size ,_buildingType, buildStats.buildSupply);
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