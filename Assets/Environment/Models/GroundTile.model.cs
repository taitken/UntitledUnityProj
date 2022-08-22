using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Environment.Models
{
    public class GroundTileModel : TileObjectModel
    {
        public eGroundTypes groundType {get;set;}
        public GroundTileModel(Vector3Int _position, IList<ItemObjectMass> _item, eGroundTypes _groundType) :base(_position, _item)
        {
            this.groundType = _groundType;
        }

        public enum eGroundTypes{
            grass,
            dirt
        }
    }
}

