using System;
using UnityEngine;

namespace Environment.Models
{
    public class GroundTileModel : TileObjectModel
    {
        public eGroundTypes groundType {get;set;}
        public GroundTileModel(Vector3Int _position, decimal _weight, eGroundTypes _groundType) :base(_position, _weight)
        {
            this.groundType = _groundType;
        }

        public enum eGroundTypes{
            grass,
            dirt
        }
    }
}

