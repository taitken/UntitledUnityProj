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
        public RoomModel(IList<FloorTileModel> _connectedTiles )
        {
            connectedTiles = _connectedTiles;
        }

        private IList<FloorTileModel> connectedTiles { get; set; }

    }
}

