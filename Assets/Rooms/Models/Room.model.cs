using System;
using UnityEngine;
using System.Collections.Generic;
using Item.Models;
using GameControllers.Models;
using ObjectComponents;
using Building.Models;

namespace Room.Models
{
    public class RoomModel : BaseModel
    {

        public eFloorType floorType { get; set; }
        public IList<FloorTileModel> connectedTiles { get; set; }
        public IList<BorderTileModel> borderTiles { get; set; }
        public int size { get { return connectedTiles.Count; } }
        public int enclosedBorders { get { return borderTiles.Filter(tile => { return tile.tileSurrounded; }).Count; } }
        public RoomModel(IList<FloorTileModel> _connectedTiles, BuildingObjectModel[,] floorMap)
        {
            if (_connectedTiles != null && _connectedTiles.Count > 0)
            {
                this.borderTiles = new List<BorderTileModel>();
                this.connectedTiles = _connectedTiles;
                this.floorType = _connectedTiles[0].floorType;
                this.connectedTiles.ForEach(tile =>
                {
                    if (this.TileOnBorder(floorMap, tile.position))
                    {
                        borderTiles.Add(new BorderTileModel(tile, floorMap));
                    }
                });
            }
        }

        public void UpdateRoomModel(RoomModel newRoomModel)
        {
            this.floorType = newRoomModel.floorType;
            this.connectedTiles = newRoomModel.connectedTiles;
            this.borderTiles = newRoomModel.borderTiles;
        }

        private bool TileOnBorder(BuildingObjectModel[,] floorMap, Vector3Int pos)
        {
            return FloorTileCheck(floorMap[pos.x + 1, pos.y]) || FloorTileCheck(floorMap[pos.x - 1, pos.y]) ||
            FloorTileCheck(floorMap[pos.x, pos.y + 1]) || FloorTileCheck(floorMap[pos.x + 1, pos.y - 1]);
        }

        private bool FloorTileCheck(BuildingObjectModel building)
        {
            if (building == null) return true;
            if (building.buildingCategory != eBuildingCategory.FloorTile) return true;
            return false;
        }

    }
}

