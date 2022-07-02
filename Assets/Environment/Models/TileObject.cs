using System;
using UnityEngine;

namespace Environment.Models
{
    public class TileObject : BaseModel
    {
        public TileObject(Vector3Int _position) :base()
        {
            this.position = _position;
        }
        public Vector3Int position { get; set; }
    }
}

