using System;
using System.Collections.Generic;
using Item.Models;
using MineableBlocks.Models;
using ObjectComponents;
using UnityEngine;

namespace Environment.Models
{
    public class MineableObjectModel : TileObjectModel
    {
        public eMineableBlockType mineableBlockType;
        public eItemType itemDrop;
        public ObjectHitPointsComponent hitPointsComponent;
        public string blockName;
        public MineableObjectModel(Vector3Int _position, eMineableBlockType _blockType, MineableBlockStatsModel _blockStats)
            : base(_position, new List<ItemObjectMass> { new ItemObjectMass(_blockStats.dropType, UnityEngine.Random.Range(_blockStats.minMass, _blockStats.maxMass)) })
        {
            this.mineableBlockType = _blockType;
            this.itemDrop = _blockStats.dropType;
            this.blockName = _blockStats.name;            
            this.hitPointsComponent = new ObjectHitPointsComponent(_blockStats.hitPoints);
            this.objectComponents.Add(this.hitPointsComponent);
        }
    }
}

