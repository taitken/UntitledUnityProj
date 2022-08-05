using System;
using Item.Models;
using MineableBlocks.Models;
using UnityEngine;

namespace Environment.Models
{
    public class MineableObjectModel : TileObjectModel
    {
        public eMineableBlockType mineableBlockType;
        public eItemType itemDrop;
        public string blockName;
        public MineableObjectModel(Vector3Int _position, eMineableBlockType _blockType, MineableBlockStatsModel _blockStats)
            : base(_position, UnityEngine.Random.Range(_blockStats.minMass, _blockStats.maxMass))
        {
            this.mineableBlockType = _blockType;
            this.itemDrop = _blockStats.dropType;
            this.blockName = _blockStats.name;
        }
    }
}

