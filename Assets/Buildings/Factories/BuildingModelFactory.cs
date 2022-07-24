using System;
using GameControllers.Models;
using GameControllers.Services;
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
            switch (_buildingType)
            {
                case eBuildingType.Chest:
                    newBuilding = new ChestBuildingModel(_position, _buildingType);
                    break;
                case eBuildingType.FloorTile:
                    newBuilding = new FloorTileModel(_position, _buildingType);
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