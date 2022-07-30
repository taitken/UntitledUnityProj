
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
        private MineableHunk.Factory hunkFactory;
        private IList<MineableHunk> mineableHunks = new List<MineableHunk>();
        private IList<MineableObjectModel> mineableObjectModels
        {
            get
            {
                return this.mineableHunks.Map(hunk => { return hunk.mineableObjectModel; });
            }
        }
        private MineableHunk[,] hunkMap
        {
            get
            {
                MineableHunk[,] hunkMap = new MineableHunk[MAP_WIDTH, MAP_HEIGHT];
                this.mineableHunks.ForEach(hunk =>
                {
                    hunkMap[hunk.mineableObjectModel.position.x, hunk.mineableObjectModel.position.y] = hunk;
                });
                return hunkMap;
            }
        }

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              MineableHunk.Factory _hunkFactory,
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
            this.subscriptions.Add(this.environmentService.mineableObjects.Subscribe(mineableObj =>
            {
                this.refreshMinables(mineableObj);
            }));
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
                    this.mineableHunks.Add(this.CreateMineableObject(mineableObj));
                });
                objsToRemove.ForEach(hunkModel =>
                {
                    MineableHunk hunk = this.mineableHunks.Find(hunk => { return hunk.mineableObjectModel.ID == hunkModel.ID; });
                    this.mineableHunks.Remove(hunk);
                    hunk.Destroy();
                });
                this.UpdateTileMap();
            }
        }

        private MineableHunk CreateMineableObject(MineableObjectModel mineableObj)
        {
            MineableHunk newHunk = this.hunkFactory.Create(mineableObj);
            newHunk.transform.position = this.tilemap.CellToLocal(mineableObj.position);
            return newHunk;
        }

        private void ReMapSpries()
        {
            MineableHunk[,] hunkMapArray = this.hunkMap;
            // Very inefficient implementation
            // -- To redo
            foreach (MineableHunk hunk in this.mineableHunks)
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

        private bool HunkExistsInPosition(int xPos, int yPos, MineableHunk[,] hunkMapArray)
        {
            if (xPos < 0 || xPos >= hunkMapArray.GetLength(0)) return false;
            if (yPos < 0 || yPos >= hunkMapArray.GetLength(1)) return false;
            return hunkMapArray[xPos, yPos] != null;
        }
    }
}