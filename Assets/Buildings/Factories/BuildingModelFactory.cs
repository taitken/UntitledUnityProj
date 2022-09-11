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
            switch (_buildingType)
            {
                case eBuildingType.Chest:
                    newBuilding = new StorageBuildingModel(_position, _buildingType, buildStats);
                    break;
                case eBuildingType.FloorTile:
                    newBuilding = new FloorTileModel(_position, _buildingType, buildStats);
                    break;
                case eBuildingType.Smelter:
                    newBuilding = new ProductionBuildingModel(_position, _buildingType, buildStats);
                    break;
                case eBuildingType.Torch:
                    newBuilding = new DecorBuildingModel(_position, _buildingType, buildStats);
                    break;
                case eBuildingType.Wall:
                    newBuilding = new WallBuildingModel(_position, _buildingType, buildStats);
                    break;
                case eBuildingType.Door:
                    newBuilding = new DoorBuildingModel(_position, _buildingType, buildStats);
                    break;
                case eBuildingType.FarmPlot:
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