using System;
using UnityEngine;

namespace Environment.Models
{
    public class MineableObjectModel : TileObject
    {
        public MineableObjectModel(Vector3Int _position) : base(_position)
        {
        }
        public long weight { get; set; } = 400;
    }
}

