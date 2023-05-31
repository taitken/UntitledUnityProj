using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Room.Models;
using Building.Models;

namespace GameControllers.Services
{
    public class RoomService : BaseService, IRoomService
    {
        public RoomService()
        {

        }
        public MonoObseravable<IList<RoomModel>> roomObservable { get; set; } = new MonoObseravable<IList<RoomModel>>(new List<RoomModel>());

        public void AddRoom(RoomModel newRoom)
        {
            IList<RoomModel> _rooms = this.roomObservable.Get();
            if (newRoom != null && _rooms.Find(existingRoom => { return newRoom.ID == existingRoom.ID; }) == null)
            {
                // Check if tiles already exist in a room.
                IList<RoomModel> matchedRooms = this.FindMatchingRooms(newRoom, _rooms);
                if (matchedRooms.Count == 0)
                {
                    _rooms.Add(newRoom);
                    this.roomObservable.Set(_rooms);
                }
                // Merge rooms
                else
                {
                    RoomModel firstExistingRoom = matchedRooms[0];
                    if (matchedRooms.Count > 1)
                    {
                        matchedRooms.ForEach((room, index) => { if (index != 0) this.RemoveRoom(room.ID); });
                    }
                    firstExistingRoom.UpdateRoomModel(newRoom);
                    this.roomObservable.NotifyAllSubscribers();

                }
            }
        }

        public IList<RoomModel> GetRooms()
        {
            return this.roomObservable.Get();
        }
        public IList<RoomModel> GetRooms(eFloorType _roomType)
        {
            return this.roomObservable.Get().Filter(room => { return room.floorType == _roomType; });
        }

        public RoomModel GetRoom(FloorTileModel _floorTile)
        {
            IList<RoomModel> rooms = this.GetRooms(_floorTile.floorType);
            RoomModel matchingRoom = null;
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].connectedTiles.Find(tile => { return tile.ID == _floorTile.ID; }) != null)
                {
                    matchingRoom = rooms[i];
                    break;
                }
            };
            return matchingRoom;
        }

        private IList<RoomModel> FindMatchingRooms(RoomModel newRoom, IList<RoomModel> existingRooms)
        {
            // Should implement a two pass system. First pass checks first 5 tiles in each room for a quick match.
            // Second should do a detailed scan.
            IList<RoomModel> matchedRooms = new List<RoomModel>();
            if (newRoom.connectedTiles != null && newRoom.connectedTiles.Count > 0)
            {
                existingRooms.Filter(existingRoom => { return existingRoom.floorType == newRoom.floorType; }).ForEach(existingRoom =>
                {
                    bool matchFound = false;
                    for (int i = 0; i < existingRoom.connectedTiles.Count; i++)
                    {
                        matchFound = newRoom.connectedTiles.Find(newTile => { return newTile.ID == existingRoom.connectedTiles[i].ID; }) != null;
                        if (matchFound)
                        {
                            matchedRooms.Add(existingRoom);
                            break;
                        }
                    }
                });
            }
            return matchedRooms;
        }

        public void RemoveRoom(long id)
        {
            this.roomObservable.Set(this.roomObservable.Get().Filter(order => { return order.ID != id; }));
        }

        public RoomModel FindRoom(BuildingObjectModel[,] floorTileMap, FloorTileModel startingTile)
        {
            RoomModel newRoom;
            IList<FloorTileModel> connectedTiles = new List<FloorTileModel>();
            connectedTiles.Add(startingTile);
            for (int i = 0; i < connectedTiles.Count; i++)
            {
                IList<BuildingObjectModel> potentialTiles = new List<BuildingObjectModel>();
                potentialTiles.Add(floorTileMap[connectedTiles[i].position.x, connectedTiles[i].position.y + 1]);
                potentialTiles.Add(floorTileMap[connectedTiles[i].position.x, connectedTiles[i].position.y - 1]);
                potentialTiles.Add(floorTileMap[connectedTiles[i].position.x + 1, connectedTiles[i].position.y]);
                potentialTiles.Add(floorTileMap[connectedTiles[i].position.x - 1, connectedTiles[i].position.y]);

                potentialTiles.ForEach(potentialTile =>
                {
                    // If tile exists, its a floor tile and its not in the list already
                    if (potentialTile != null && potentialTile.buildingCategory == eBuildingCategory.FloorTile 
                    && connectedTiles.Find(existingTile => { return existingTile == potentialTile; }) == null)
                    {
                        connectedTiles.Add(potentialTile as FloorTileModel);
                    }
                });
            }
            newRoom = new RoomModel(connectedTiles, floorTileMap);

            return newRoom;
        }
    }
}
