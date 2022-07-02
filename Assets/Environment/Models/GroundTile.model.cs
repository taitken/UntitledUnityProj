using System;
using UnityEngine;

namespace Environment.Models
{
    public class GroundTileModel : TileObject
    {
        public eGroundTypes groundType {get;set;}
        public GroundTileModel(Vector3Int _position, eGroundTypes _groundType) :base(_position)
        {
            this.groundType = _groundType;
        }

        public enum eGroundTypes{
            grass,
            dirt
        }
    }
}

