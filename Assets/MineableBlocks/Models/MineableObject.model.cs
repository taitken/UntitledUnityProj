using System;
using MineableBlocks.Models;
using UnityEngine;

namespace Environment.Models
{
    public class MineableObjectModel : TileObjectModel
    {
        public eMineableBlockType mineableBlockType;
        public MineableObjectModel(Vector3Int _position, eMineableBlockType _blockType, decimal _weight) : base(_position, _weight)
        {
            this.mineableBlockType = _blockType;
        }
    }
}

