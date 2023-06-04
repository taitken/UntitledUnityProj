using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Room.Models;
using Building.Models;

namespace GameControllers.Services
{
    public interface IRoomService : IBaseService
    {
        RoomAssetController roomAssetController { get; set; }
        MonoObseravable<IList<RoomModel>> roomObservable { get; set; }
        MonoObseravable<RoomModel> selectedRoomObservable { get; set; }
        void SetRoomAssetController(RoomAssetController _roomAssetController);
        void AddRoom(RoomModel room);
        IList<RoomModel> GetRooms();
        IList<RoomModel> GetRooms(eFloorType _roomType);
        RoomModel GetRoom(FloorTileModel _floorTile);
        void RemoveRoom(long id);
        void SelectRoom(RoomModel room);
        void UnselectRoom();
        void UnselectRoom(long selectedRoomID);
        RoomModel FindRoom(BuildingObjectModel[,] floorTileMap, FloorTileModel startingTile);
    }
}

