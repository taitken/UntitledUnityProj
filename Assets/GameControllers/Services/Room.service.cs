using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Unit.Models;
using Building.Models;

namespace GameControllers.Services
{
    public class RoomService : BaseService, IRoomService
    {
        public RoomService()
        {

        }
        public MonoObseravable<IList<RoomModel>> roomObservable { get; set; } = new MonoObseravable<IList<RoomModel>>(new List<RoomModel>());

        public void AddRoom(RoomModel room)
        {
            IList<RoomModel> _rooms = this.roomObservable.Get();
            if (room != null && _rooms.Find(existingRoom => { return room.ID == existingRoom.ID; }) == null)
            {
                _rooms.Add(room);
                this.roomObservable.Set(_rooms);
            }
        }

        public void RemoveRoom(long id)
        {
            this.roomObservable.Set(this.roomObservable.Get().Filter(order => { return order.ID != id; }));
        }

        public RoomModel FindRoom(FloorTileModel[,] floorTileMap, FloorTileModel startingTile)
        {
            RoomModel newRoom;
            IList<FloorTileModel> connectedTiles = new List<FloorTileModel>();
            connectedTiles.Add(startingTile);
            for (int i = 0; i < connectedTiles.Count; i++)
            {
                IList<FloorTileModel> potentialTiles = new List<FloorTileModel>();
                potentialTiles.Add(floorTileMap[connectedTiles[i].position.x , connectedTiles[i].position.y + 1]);
                potentialTiles.Add(floorTileMap[connectedTiles[i].position.x , connectedTiles[i].position.y - 1]);
                potentialTiles.Add(floorTileMap[connectedTiles[i].position.x + 1, connectedTiles[i].position.y]);
                potentialTiles.Add(floorTileMap[connectedTiles[i].position.x - 1, connectedTiles[i].position.y]);

                potentialTiles.ForEach(potentialTile =>{
                    if(potentialTile != null && connectedTiles.Find(existingTile =>{return existingTile == potentialTile;}) == null)
                    {
                        connectedTiles.Add(potentialTile);
                    }
                });

            }
            newRoom = new RoomModel(connectedTiles);

            return newRoom;
        }
    }
}
