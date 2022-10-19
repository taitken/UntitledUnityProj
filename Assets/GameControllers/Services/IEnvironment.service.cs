using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Environment.Models;
using UnityEngine.Tilemaps;
using MineableBlocks.Models;

namespace GameControllers.Services
{
    public interface IEnvironmentService : IBaseService
    {

        public static float TILE_WIDTH_PIXELS = 0.16f;
        MonoObseravable<MineableObjectModel[,]> mineableObjects { get; set; }
        MonoObseravable<IList<GroundTileModel>> groundTiles { get; set; }
        Tilemap tileMapRef { get; set; }
        void SetMineableBlockAssetController(MineableBlockAssetController _mineableBlockAssetController);
        Sprite[] GetMineableBlockSprites(eMineableBlockType blockType);
        void AddMineableObject(MineableObjectModel mineableObject);
        void RemoveMineableObject(Vector3Int _position);
        MonoObseravable<FogModel[,]> GetFogObservable();
        void AddFogObject(FogModel fogModel);
        void RemoveFogObject(Vector3Int _position);
        IList<Vector3Int> GetCellsInArea(Vector3 startPos, Vector3 endPos);
        IList<Vector3Int> GetCellsInArea(Vector3Int startPos, Vector3Int endPos);
        Vector3Int LocalToCell(Vector3 localPosition);
        Vector3 CellToLocal(Vector3Int localPosition);
    }
}
