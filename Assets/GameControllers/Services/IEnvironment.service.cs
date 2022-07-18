using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Environment.Models;
using UnityEngine.Tilemaps;

namespace GameControllers.Services
{
    public interface IEnvironmentService
    {
        Obseravable<IList<MineableObjectModel>> mineableObjects { get; set; }
        Obseravable<IList<GroundTileModel>> groundTiles { get; set; }
        Tilemap tileMapRef { get; set; }
        void AddMineableObject(MineableObjectModel mineableObject);
        void RemoveMineableObject(long id);
        Vector3Int LocalToCell(Vector3 localPosition);
        Vector3 CellToLocal(Vector3Int localPosition);
    }
}
