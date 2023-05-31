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
        public MonoObseravable<IList<RoomModel>> roomObservable { get; set; }
        void AddRoom(RoomModel room);
        IList<RoomModel> GetRooms();
        IList<RoomModel> GetRooms(eFloorType _roomType);
        RoomModel GetRoom(FloorTileModel _floorTile);
        void RemoveRoom(long id);
        RoomModel FindRoom(BuildingObjectModel[,] floorTileMap, FloorTileModel startingTile);
    }
}

