using System.Collections;
using System.Collections.Generic;
using Building.Models;
using GameControllers.Services;
using Room.Models;
using UnityEngine;
using Zenject;

namespace GameControllers
{
    public class RoomController : MonoBehaviour2
    {
        IRoomService roomService;
        IBuildingService buildingService;
        IEnvironmentService envService;

        [Inject]
        public void Construct(IRoomService _roomService,
                              IBuildingService _buildingService,
                              IEnvironmentService _envService)
        {
            this.roomService = _roomService;
            this.buildingService = _buildingService;
            this.envService = _envService;
        }

        void Start()
        {
            this.buildingService.SubscribeToNewBuildingTrigger(this, this.MonitorNewBuilding);
        }

        void MonitorNewBuilding(BuildingObjectModel newBuildingObjectModel)
        {
            if (newBuildingObjectModel.buildingCategory == eBuildingCategory.FloorTile)
            {
                this.CheckForNewRoomCreation(newBuildingObjectModel as FloorTileModel);
            }
            if (newBuildingObjectModel.buildingCategory == eBuildingCategory.Wall || newBuildingObjectModel.buildingCategory == eBuildingCategory.Door)
            {
                this.buildingService.buildingObseravable.Get().Filter(building => { return building.buildingCategory == eBuildingCategory.FloorTile; }).ForEach(tile =>
                {
                    if (tile.position.x == newBuildingObjectModel.position.x + 1 && tile.position.y == newBuildingObjectModel.position.y
                        || tile.position.x == newBuildingObjectModel.position.x - 1 && tile.position.y == newBuildingObjectModel.position.y
                        || tile.position.x == newBuildingObjectModel.position.x && tile.position.y == newBuildingObjectModel.position.y + 1
                        || tile.position.x == newBuildingObjectModel.position.x && tile.position.y == newBuildingObjectModel.position.y - 1)
                    {
                        this.CheckForNewRoomCreation(tile as FloorTileModel);
                    }
                });
            }
        }

        void CheckForNewRoomCreation(FloorTileModel floorTileModel)
        {
            if (floorTileModel.buildingCategory == eBuildingCategory.FloorTile)
            {
                IList<FloorTileModel> floorTiles = new List<FloorTileModel>();
                IList<WallBuildingModel> wallModels = new List<WallBuildingModel>();
                IList<DoorBuildingModel> doorModel = new List<DoorBuildingModel>();
                this.buildingService.buildingObseravable.Get().ForEach(building =>
                {
                    if (building.buildingCategory == eBuildingCategory.FloorTile && building.buildingType == floorTileModel.buildingType)
                    {
                        floorTiles.Add(building as FloorTileModel);
                    }
                    if (building.buildingCategory == eBuildingCategory.Wall)
                    {
                        wallModels.Add(building as WallBuildingModel);
                    }
                    if (building.buildingCategory == eBuildingCategory.Door)
                    {
                        doorModel.Add(building as DoorBuildingModel);
                    }
                });

                // Map floor plan. Overriding floor tiles with walls and doors.
                BuildingObjectModel[,] buildingObjectModels = new BuildingObjectModel[MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT];
                floorTiles.ForEach(tile =>
                {
                    buildingObjectModels[tile.position.x, tile.position.y] = tile;
                });
                wallModels.ForEach(wall =>
                {
                    buildingObjectModels[wall.position.x, wall.position.y] = wall;
                });
                doorModel.ForEach(door =>
                {
                    buildingObjectModels[door.position.x, door.position.y] = door;
                });

                // Check input floor tile is not covered.
                if (buildingObjectModels[floorTileModel.position.x, floorTileModel.position.y].buildingCategory != eBuildingCategory.FloorTile)
                {
                    floorTiles = floorTiles.Filter(tile =>{return tile.ID != floorTileModel.ID;});
                    if(floorTiles.Count == 0) return;
                    floorTileModel = floorTiles[0];
                }
                RoomModel newRoom = this.roomService.FindRoom(buildingObjectModels, floorTileModel);
                this.roomService.AddRoom(newRoom);
            }
        }
    }
}