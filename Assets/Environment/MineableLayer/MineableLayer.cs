
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
        private IList<MineableBlock> MineableBlocks = new List<MineableBlock>();
        private IList<MineableObjectModel> mineableObjectModels
        {
            get
            {
                return this.MineableBlocks.Map(hunk => { return hunk.mineableObjectModel; });
            }
        }
        private MineableBlock[,] hunkMap
        {
            get
            {
                MineableBlock[,] hunkMap = new MineableBlock[MAP_WIDTH, MAP_HEIGHT];
                this.MineableBlocks.ForEach(hunk =>
                {
                    hunkMap[hunk.mineableObjectModel.position.x, hunk.mineableObjectModel.position.y] = hunk;
                });
                return hunkMap;
            }
        }

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
            this.environmentService.mineableObjects.Subscribe(this, mineableObj => { this.refreshMinables(mineableObj); });
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void UpdateTileMap()
        {
            this.ReMapSpries();
        }

        private void refreshMinables(IList<MineableObjectModel> mineableObjs)
        {
            if (this.tilemap != null)
            {
                IList<MineableObjectModel> objsToAdd = mineableObjs.GetNewModels(this.mineableObjectModels);
                IList<MineableObjectModel> objsToRemove = mineableObjs.GetRemovedModels(this.mineableObjectModels);
                objsToAdd.ForEach(mineableObj =>
                {
                    this.MineableBlocks.Add(this.CreateMineableObject(mineableObj));
                });
                objsToRemove.ForEach(hunkModel =>
                {
                    MineableBlock hunk = this.MineableBlocks.Find(hunk => { return hunk.mineableObjectModel.ID == hunkModel.ID; });
                    this.MineableBlocks.Remove(hunk);
                    hunk.Destroy();
                });
                this.UpdateTileMap();
            }
        }

        private MineableBlock CreateMineableObject(MineableObjectModel mineableObj)
        {
            MineableBlock newHunk = this.hunkFactory.Create(mineableObj);
            newHunk.transform.position = this.tilemap.CellToLocal(mineableObj.position);
            return newHunk;
        }

        private void ReMapSpries()
        {
            MineableBlock[,] hunkMapArray = this.hunkMap;
            // Very inefficient implementation
            // -- To redo
            foreach (MineableBlock hunk in this.MineableBlocks)
            {
                Vector3Int cellPos = this.tilemap.LocalToCell(hunk.gameObject.transform.localPosition);
                bool x0y0 = this.HunkExistsInPosition(cellPos.x - 1, cellPos.y + 1, hunkMapArray);
                bool x1y0 = this.HunkExistsInPosition(cellPos.x, cellPos.y + 1, hunkMapArray);
                bool x2y0 = this.HunkExistsInPosition(cellPos.x + 1, cellPos.y + 1, hunkMapArray);
                bool x0y1 = this.HunkExistsInPosition(cellPos.x - 1, cellPos.y, hunkMapArray);
                bool x2y1 = this.HunkExistsInPosition(cellPos.x + 1, cellPos.y, hunkMapArray);
                bool x0y2 = this.HunkExistsInPosition(cellPos.x - 1, cellPos.y - 1, hunkMapArray);
                bool x1y2 = this.HunkExistsInPosition(cellPos.x, cellPos.y - 1, hunkMapArray);
                bool x2y2 = this.HunkExistsInPosition(cellPos.x + 1, cellPos.y - 1, hunkMapArray);
                hunk.updateSprite(SpriteTileMapping.getMapping(x0y0, x1y0, x2y0, x0y1, x2y1, x0y2, x1y2, x2y2));
            }
        }

        private bool HunkExistsInPosition(int xPos, int yPos, MineableBlock[,] hunkMapArray)
        {
            if (xPos < 0 || xPos >= hunkMapArray.GetLength(0)) return false;
            if (yPos < 0 || yPos >= hunkMapArray.GetLength(1)) return false;
            return hunkMapArray[xPos, yPos] != null;
        }
    }
}