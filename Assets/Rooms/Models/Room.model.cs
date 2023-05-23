using System;
using UnityEngine;
using System.Collections.Generic;
using Item.Models;
using GameControllers.Models;
using ObjectComponents;
using Building.Models;

namespace Unit.Models
{
    public class RoomModel : BaseModel
    {
        public RoomModel(IList<FloorTileModel> _connectedTiles)
        {
            if (_connectedTiles != null && _connectedTiles.Count > 0)
            {
                connectedTiles = _connectedTiles;
                floorType = _connectedTiles[0].floorType;
            }
        }

        public eFloorType floorType { get; set; }
        public IList<FloorTileModel> connectedTiles { get; set; }

    }
}

