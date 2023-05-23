using System.Collections;
using System.Collections.Generic;
using Building.Models;
using GameControllers.Services;
using Unit.Models;
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
            this.buildingService.SubscribeToNewBuildingTrigger(this, this.CheckForNewRoomCreation);
        }

        void CheckForNewRoomCreation(BuildingObjectModel newBuildingObjectModel)
        {
            if (newBuildingObjectModel.buildingCategory == eBuildingCategory.FloorTile)
            {
                IList<FloorTileModel> floorTiles = this.buildingService.buildingObseravable.Get()
                    .Filter(building => { return building.buildingCategory == eBuildingCategory.FloorTile && building.buildingType == newBuildingObjectModel.buildingType; })
                    .Map(building => { return building as FloorTileModel; });

                FloorTileModel[,] newFloorTileModel = new FloorTileModel[MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT];
                floorTiles.ForEach(tile =>{
                    newFloorTileModel[tile.position.x, tile.position.y] = tile;
                });

                RoomModel newRoom = this.roomService.FindRoom(newFloorTileModel, newBuildingObjectModel as FloorTileModel);
                this.roomService.AddRoom(newRoom);
            }
        }
    }
}