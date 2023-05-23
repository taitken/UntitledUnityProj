using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Unit.Models;
using Building.Models;

namespace GameControllers.Services
{
    public interface IRoomService : IBaseService
    {
        public MonoObseravable<IList<RoomModel>> roomObservable { get; set; }
        void AddRoom(RoomModel room);
        void RemoveRoom(long id);
        RoomModel FindRoom(FloorTileModel[,] floorTileMap, FloorTileModel startingTile);
    }
}

