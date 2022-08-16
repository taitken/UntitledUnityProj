
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Environment
{
    public class SpriteTileMapping
    {
        // 0 = must be empty
        // 1 = empty or occupied
        // 2 = must be occupied
        public static readonly IList<TileSpriteMap> spriteMap = new List<TileSpriteMap>{
        new TileSpriteMap(0, 0, 0, 0, 0, 0, 0, 2, 0),
        new TileSpriteMap(1, 1, 0, 1, 0, 2, 1, 2, 2),
        new TileSpriteMap(2, 1, 0, 1, 2, 2, 1, 2, 1),
        new TileSpriteMap(3, 1, 0, 1, 2, 0, 2, 2, 1),
        new TileSpriteMap(4, 2, 2, 1, 2, 2, 2, 2, 0),
        new TileSpriteMap(5, 2, 2, 2, 2, 2, 0, 2, 2),
        new TileSpriteMap(6, 1, 2, 1, 0, 0, 1, 2, 1),
        new TileSpriteMap(7, 1, 2, 2, 0, 2, 1, 2, 2),
        new TileSpriteMap(8, 2, 2, 2, 2, 2, 2, 2, 2),
        new TileSpriteMap(9, 2, 2, 1, 2, 0, 2, 2, 1),
        new TileSpriteMap(10, 2, 2, 0, 2, 2, 2, 2, 2),
        new TileSpriteMap(11, 0, 2, 2, 2, 2, 2, 2, 2),
        new TileSpriteMap(12, 1, 2, 1, 0, 0, 0, 0, 0),
        new TileSpriteMap(13, 1, 2, 2, 0, 2, 1, 0, 1),
        new TileSpriteMap(14, 2, 2, 2, 2, 2, 1, 0, 1),
        new TileSpriteMap(15, 2, 2, 1, 2, 0, 1, 0, 1),
        new TileSpriteMap(16, 0, 2, 1, 2, 0, 2, 2, 1),
        new TileSpriteMap(17, 1, 2, 0, 0, 2, 1, 2, 2),
        new TileSpriteMap(18, 1, 0, 1, 0, 0, 1, 0, 1),
        new TileSpriteMap(19, 1, 0, 1, 0, 2, 1, 0, 1),
        new TileSpriteMap(20, 1, 0, 1, 2, 2, 1, 0, 1),
        new TileSpriteMap(21, 1, 0, 1, 2, 0, 1, 0, 1),
        new TileSpriteMap(37, 1, 2, 1, 0, 2, 1, 2, 1),
        new TileSpriteMap(22, 0, 0, 0, 0, 2, 0, 2, 0),
        new TileSpriteMap(24, 0, 0, 0, 2, 0, 0, 2, 0),
        new TileSpriteMap(25, 1, 0, 1, 2, 2, 0, 2, 0),
        new TileSpriteMap(27, 0, 2, 1, 2, 0, 0, 2, 1),
        new TileSpriteMap(28, 0, 2, 0, 0, 0, 1, 2, 1),
        new TileSpriteMap(33, 1, 2, 0, 0, 2, 1, 0, 1),
        new TileSpriteMap(34, 0, 2, 0, 0, 2, 0, 0, 0),
        new TileSpriteMap(35, 0, 2, 1, 2, 0, 1, 0, 1),
        new TileSpriteMap(36, 0, 2, 0, 2, 0, 0, 0, 0),
        new TileSpriteMap(37, 1, 2, 0, 0, 2, 1, 2, 0),
        new TileSpriteMap(39, 0, 2, 0, 2, 2, 1, 0, 1)
        };

        public static int getMapping(bool _x0y0, bool _x1y0, bool _x2y0,
                                             bool _x0y1, bool _x2y1,
                                             bool _x0y2, bool _x1y2, bool _x2y2)
        {
            return SpriteTileMapping.spriteMap.Find(map =>
            {
                return (map.x0y0 == 1 || map.x0y0 == (_x0y0 ? 2 : 0)) &&
                        (map.x0y1 == 1 || map.x0y1 == (_x0y1 ? 2 : 0)) &&
                        (map.x0y2 == 1 || map.x0y2 == (_x0y2 ? 2 : 0)) &&
                        (map.x1y0 == 1 || map.x1y0 == (_x1y0 ? 2 : 0)) &&
                        (map.x1y2 == 1 || map.x1y2 == (_x1y2 ? 2 : 0)) &&
                        (map.x2y0 == 1 || map.x2y0 == (_x2y0 ? 2 : 0)) &&
                        (map.x2y1 == 1 || map.x2y1 == (_x2y1 ? 2 : 0)) &&
                        (map.x2y2 == 1 || map.x2y2 == (_x2y2 ? 2 : 0));
            }).spriteRef;
        }
        public static bool HunkExistsInPosition<Type>(int xPos, int yPos, Type[,] hunkMapArray)
        {
            return hunkMapArray.ValidIndex(xPos, yPos) && hunkMapArray[xPos, yPos] != null;
        }
    }

    public struct TileSpriteMap
    {

        public TileSpriteMap(int _spriteRef, int _x0y0, int _x1y0, int _x2y0,
                                             int _x0y1, int _x2y1,
                                             int _x0y2, int _x1y2, int _x2y2)
        {
            this.spriteRef = _spriteRef;
            this.x0y0 = _x0y0;
            this.x0y1 = _x0y1;
            this.x0y2 = _x0y2;
            this.x1y0 = _x1y0;
            this.x1y2 = _x1y2;
            this.x2y0 = _x2y0;
            this.x2y1 = _x2y1;
            this.x2y2 = _x2y2;
        }
        public readonly int spriteRef;
        public readonly int x0y0;
        public readonly int x0y1;
        public readonly int x0y2;
        public readonly int x1y0;
        public readonly int x1y2;
        public readonly int x2y0;
        public readonly int x2y1;
        public readonly int x2y2;
    }
}