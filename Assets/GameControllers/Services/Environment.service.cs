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
        private MonoObseravable<FogModel[,]> fogModels { get; set; } = new MonoObseravable<FogModel[,]>(null);
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

        public MonoObseravable<FogModel[,]> GetFogObservable()
        {
            return this.fogModels;
        }

        public void AddFogObject(FogModel fogModel)
        {
            FogModel[,] _fogModels = this.fogModels.Get();
            if (fogModel != null && _fogModels[fogModel.position.x, fogModel.position.y] == null)
            {
                _fogModels[fogModel.position.x, fogModel.position.y] = fogModel;
                this.fogModels.Set(_fogModels);
            }
        }
        public void RemoveFogObject(Vector3Int _position)
        {
            FogModel[,] _fogModels = this.fogModels.Get();
            _fogModels[_position.x, _position.y] = null;
            this.fogModels.Set(_fogModels);
        }

        public IList<Vector3Int> GetCellsInArea(Vector3 startPos, Vector3 endPos)
        {
            return GetCellsInArea(this.LocalToCell(startPos), this.LocalToCell(endPos));
        }

        public IList<Vector3Int> GetCellsInArea(Vector3Int startPos, Vector3Int endPos)
        {
            List<Vector3Int> draggedCells = new List<Vector3Int>();
            int xDistance = endPos.x - startPos.x;
            int yDistance = endPos.y - startPos.y;
            int xSign = Math.Sign(xDistance);
            int ySign = Math.Sign(yDistance);
            for (int x = 0; x <= Math.Abs(xDistance); x++)
            {
                for (int y = 0; y <= Math.Abs(yDistance); y++)
                {
                    draggedCells.Add(new Vector3Int(startPos.x + (x * xSign), startPos.y + (y * ySign)));
                }
            }
            return draggedCells;
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