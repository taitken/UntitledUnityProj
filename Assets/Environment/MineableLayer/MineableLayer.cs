
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Environment.Models;
using GameControllers.Services;
using Extensions;
using Zenject;

namespace Environment
{
    public class MineableLayer : MonoBehaviourLayer
    {
        private IUnitOrderService orderService;
        private IEnvironmentService environmentService;
        private MineableBlock.Factory hunkFactory;
        private MineableBlock[,] mineableBlocks;

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              MineableBlock.Factory _hunkFactory,
                              IEnvironmentService _environmentService,
                              LayerCollider.Factory _layerColliderFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "MineableLayer");
            this.hunkFactory = _hunkFactory;
            this.orderService = _orderService;
            this.environmentService = _environmentService;
        }

        // Start is called before the first frame update
        void Start()
        {
            this.tilemap = GetComponent<Tilemap>();
            this.environmentService.mineableObjects.Subscribe(this, mineableObjs =>
            {
                if (mineableObjs != null)
                {
                    if (this.mineableBlocks == null)
                    {
                        this.mineableBlocks = new MineableBlock[mineableObjs.GetLength(0), mineableObjs.GetLength(1)];
                        this.RefreshMinables(mineableObjs);
                        this.ReMapSprites(this.mineableBlocks);
                    }
                    else
                    {
                        this.RefreshMinables(mineableObjs);
                    }
                }
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void RefreshMinables(MineableObjectModel[,] mineableObjs)
        {
            if (this.tilemap != null)
            {
                IList<MineableObjectModel> objsToAdd = new List<MineableObjectModel>();
                IList<MineableObjectModel> objsToRemove = new List<MineableObjectModel>();
                for (int x = 0; x < mineableObjs.GetLength(0); x++)
                {
                    for (int y = 0; y < mineableObjs.GetLength(1); y++)
                    {
                        if (this.mineableBlocks[x, y] == null && mineableObjs[x, y] != null)
                        {
                            objsToAdd.Add(mineableObjs[x, y]);
                        }
                        if (this.mineableBlocks[x, y] != null && mineableObjs[x, y] == null)
                        {
                            objsToRemove.Add(mineableBlocks[x, y].mineableObjectModel);
                        }
                    }
                }
                objsToAdd.ForEach(mineableObj =>
                {
                    this.mineableBlocks[mineableObj.position.x, mineableObj.position.y] = this.CreateMineableObject(mineableObj);
                });
                objsToRemove.ForEach(mineableObj =>
                {
                    MineableBlock hunk = this.mineableBlocks[mineableObj.position.x, mineableObj.position.y];
                    this.mineableBlocks[mineableObj.position.x, mineableObj.position.y] = null;
                    hunk.Destroy();
                    this.ReMapBlocksAround(mineableObj.position);
                });
            }
        }

        private void ReMapAllSprites()
        {
            this.ReMapSprites(this.mineableBlocks);
        }

        private void ReMapBlocksAround(Vector3Int centerPoint)
        {
            MineableBlock[,] hunksToRefresh = new MineableBlock[3, 3];
            hunksToRefresh[0, 0] = this.mineableBlocks.ValidIndex(centerPoint.x - 1, centerPoint.y - 1) ? this.mineableBlocks[centerPoint.x - 1, centerPoint.y - 1] : null;
            hunksToRefresh[0, 1] = this.mineableBlocks.ValidIndex(centerPoint.x - 1, centerPoint.y) ? this.mineableBlocks[centerPoint.x - 1, centerPoint.y] : null;
            hunksToRefresh[0, 2] = this.mineableBlocks.ValidIndex(centerPoint.x - 1, centerPoint.y + 1) ? this.mineableBlocks[centerPoint.x - 1, centerPoint.y + 1] : null;
            hunksToRefresh[1, 0] = this.mineableBlocks.ValidIndex(centerPoint.x, centerPoint.y - 1) ? this.mineableBlocks[centerPoint.x, centerPoint.y - 1] : null;
            hunksToRefresh[1, 1] = this.mineableBlocks.ValidIndex(centerPoint.x, centerPoint.y) ? this.mineableBlocks[centerPoint.x, centerPoint.y] : null;
            hunksToRefresh[1, 2] = this.mineableBlocks.ValidIndex(centerPoint.x, centerPoint.y + 1) ? this.mineableBlocks[centerPoint.x, centerPoint.y + 1] : null;
            hunksToRefresh[2, 0] = this.mineableBlocks.ValidIndex(centerPoint.x + 1, centerPoint.y - 1) ? this.mineableBlocks[centerPoint.x + 1, centerPoint.y - 1] : null;
            hunksToRefresh[2, 1] = this.mineableBlocks.ValidIndex(centerPoint.x + 1, centerPoint.y) ? this.mineableBlocks[centerPoint.x + 1, centerPoint.y] : null;
            hunksToRefresh[2, 2] = this.mineableBlocks.ValidIndex(centerPoint.x + 1, centerPoint.y + 1) ? this.mineableBlocks[centerPoint.x + 1, centerPoint.y + 1] : null;
            this.ReMapSprites(hunksToRefresh);
        }

        private MineableBlock CreateMineableObject(MineableObjectModel mineableObj)
        {
            MineableBlock newHunk = this.hunkFactory.Create(mineableObj);
            newHunk.transform.position = this.tilemap.CellToLocal(mineableObj.position);
            return newHunk;
        }

        private void ReMapSprites(MineableBlock[,] hunksToRefresh)
        {
            MineableBlock[,] hunkMapArray = this.mineableBlocks;
            // Very inefficient implementation
            // -- To redo
            foreach (MineableBlock hunk in hunksToRefresh)
            {
                if (hunk != null)
                {
                    Vector3Int cellPos = this.tilemap.LocalToCell(hunk.gameObject.transform.localPosition);
                    bool x0y0 = SpriteTileMapping.HunkExistsInPosition(cellPos.x - 1, cellPos.y + 1, hunkMapArray);
                    bool x1y0 = SpriteTileMapping.HunkExistsInPosition(cellPos.x, cellPos.y + 1, hunkMapArray);
                    bool x2y0 = SpriteTileMapping.HunkExistsInPosition(cellPos.x + 1, cellPos.y + 1, hunkMapArray);
                    bool x0y1 = SpriteTileMapping.HunkExistsInPosition(cellPos.x - 1, cellPos.y, hunkMapArray);
                    bool x2y1 = SpriteTileMapping.HunkExistsInPosition(cellPos.x + 1, cellPos.y, hunkMapArray);
                    bool x0y2 = SpriteTileMapping.HunkExistsInPosition(cellPos.x - 1, cellPos.y - 1, hunkMapArray);
                    bool x1y2 = SpriteTileMapping.HunkExistsInPosition(cellPos.x, cellPos.y - 1, hunkMapArray);
                    bool x2y2 = SpriteTileMapping.HunkExistsInPosition(cellPos.x + 1, cellPos.y - 1, hunkMapArray);
                    hunk.UpdateSprite(SpriteTileMapping.getMapping(x0y0, x1y0, x2y0, x0y1, x2y1, x0y2, x1y2, x2y2));
                }
            }
        }
    }
}