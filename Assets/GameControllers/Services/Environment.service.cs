using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using UnityEngine.Tilemaps;
using Environment.Models;
using MineableBlocks.Models;

namespace GameControllers.Services
{
    public class EnvironmentService : BaseService, IEnvironmentService
    {
        public MonoObseravable<MineableObjectModel[,]> mineableObjects { get; set; } = new MonoObseravable<MineableObjectModel[,]>(null);
        public MonoObseravable<IList<GroundTileModel>> groundTiles { get; set; } = new MonoObseravable<IList<GroundTileModel>>(new List<GroundTileModel>());
        private MineableBlockAssetController mineableBlockAssetController { get; set; }
        public Tilemap tileMapRef { get; set; }

        public void SetMineableBlockAssetController(MineableBlockAssetController _mineableBlockAssetController)
        {
            this.mineableBlockAssetController = _mineableBlockAssetController;
            this.mineableBlockAssetController.Initialise();
        }

        public Sprite[] GetMineableBlockSprites(eMineableBlockType blockType)
        {
            return this.mineableBlockAssetController.GetBlockSpriteSet(blockType);
        }
        public void AddMineableObject(MineableObjectModel mineableObject)
        {
            MineableObjectModel[,] _mineableObjects = this.mineableObjects.Get();
            if (mineableObject != null && _mineableObjects[mineableObject.position.x, mineableObject.position.y] == null)
            {
                _mineableObjects[mineableObject.position.x, mineableObject.position.y] = mineableObject;
                this.mineableObjects.Set(_mineableObjects);
            }
        }
        public void RemoveMineableObject(Vector3Int _position)
        {
            MineableObjectModel[,] _mineableObjects = this.mineableObjects.Get();
            _mineableObjects[_position.x, _position.y] = null;
            this.mineableObjects.Set(_mineableObjects);
        }

        public Vector3Int LocalToCell(Vector3 localPosition)
        {
            if (this.tileMapRef != null)
            {
                return this.tileMapRef.LocalToCell(localPosition);
            }
            return new Vector3Int();
        }
        public Vector3 CellToLocal(Vector3Int localPosition)
        {
            if (this.tileMapRef != null)
            {
                return this.tileMapRef.CellToLocal(localPosition);
            }
            return new Vector3();
        }
    }
}