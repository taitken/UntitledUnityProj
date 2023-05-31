using System;
using Building.Models;

namespace Room.Models
{
    public class BorderTileModel : BaseModel
    {
        public FloorTileModel floorTile;
        public bool tileSurrounded;

        public BorderTileModel( FloorTileModel _floorTile, BuildingObjectModel[,] _roomMap)
        {
            this.floorTile = _floorTile;
            this.tileSurrounded = (_roomMap[_floorTile.position.x + 1, _floorTile.position.y] != null 
                && _roomMap[_floorTile.position.x - 1, _floorTile.position.y] != null
                && _roomMap[_floorTile.position.x, _floorTile.position.y + 1] != null
                && _roomMap[_floorTile.position.x, _floorTile.position.y - 1] != null);
        }
    }
}