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
        public MonoObseravable<IList<MineableObjectModel>> mineableObjects { get; set; } = new MonoObseravable<IList<MineableObjectModel>>(new List<MineableObjectModel>());
        public MonoObseravable<IList<GroundTileModel>> groundTiles { get; set; } = new MonoObseravable<IList<GroundTileModel>>(new List<GroundTileModel>());
        private MineableBlockAssetController mineableBlockAssetController {get;set;}
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
            IList<MineableObjectModel> _mineableObjects = this.mineableObjects.Get();
            if (mineableObject != null && _mineableObjects.Find(existingMineableObject => { return mineableObject.position == existingMineableObject.position; }) == null)
            {
                _mineableObjects.Add(mineableObject);
                this.mineableObjects.Set(_mineableObjects);
            }
        }
        public void RemoveMineableObject(long id)
        {
            this.mineableObjects.Set(this.mineableObjects.Get().Filter(mineableObject => { return mineableObject.ID != id; }));
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