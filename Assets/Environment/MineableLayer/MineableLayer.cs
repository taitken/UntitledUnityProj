
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
                    this.mineableHunks.Add(this.createMineableObject(mineableObj));
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

        private MineableHunk createMineableObject(MineableObjectModel mineableObj)
        {
            MineableHunk newHunk = this.hunkFactory.Create(mineableObj);
            newHunk.transform.position = this.tilemap.CellToLocal(mineableObj.position);
            return newHunk;
        }

        private void ReMapSpries()
        {
            // Very inefficient implementation
            // -- To redo
            foreach (MineableHunk hunk in this.mineableHunks)
            {
                Vector3Int cellPos = this.tilemap.LocalToCell(hunk.gameObject.transform.localPosition);
                bool x0y0 = this.mineableHunks.Find(delegate (MineableHunk localHunk)
                {
                    return localHunk.gameObject.transform.localPosition == this.tilemap.CellToLocal(new Vector3Int(cellPos.x - 1, cellPos.y + 1, cellPos.z));
                }) != null;
                bool x1y0 = this.mineableHunks.Find(delegate (MineableHunk localHunk)
                {
                    return localHunk.gameObject.transform.localPosition == this.tilemap.CellToLocal(new Vector3Int(cellPos.x, cellPos.y + 1, cellPos.z));
                }) != null;
                bool x2y0 = this.mineableHunks.Find(delegate (MineableHunk localHunk)
                {
                    return localHunk.gameObject.transform.localPosition == this.tilemap.CellToLocal(new Vector3Int(cellPos.x + 1, cellPos.y + 1, cellPos.z));
                }) != null;
                bool x0y1 = this.mineableHunks.Find(delegate (MineableHunk localHunk)
                {
                    return localHunk.gameObject.transform.localPosition == this.tilemap.CellToLocal(new Vector3Int(cellPos.x - 1, cellPos.y, cellPos.z));
                }) != null;
                bool x2y1 = this.mineableHunks.Find(delegate (MineableHunk localHunk)
                {
                    return localHunk.gameObject.transform.localPosition == this.tilemap.CellToLocal(new Vector3Int(cellPos.x + 1, cellPos.y, cellPos.z));
                }) != null;
                bool x0y2 = this.mineableHunks.Find(delegate (MineableHunk localHunk)
                {
                    return localHunk.gameObject.transform.localPosition == this.tilemap.CellToLocal(new Vector3Int(cellPos.x - 1, cellPos.y - 1, cellPos.z));
                }) != null;
                bool x1y2 = this.mineableHunks.Find(delegate (MineableHunk localHunk)
                {
                    return localHunk.gameObject.transform.localPosition == this.tilemap.CellToLocal(new Vector3Int(cellPos.x, cellPos.y - 1, cellPos.z));
                }) != null;
                bool x2y2 = this.mineableHunks.Find(delegate (MineableHunk localHunk)
                {
                    return localHunk.gameObject.transform.localPosition == this.tilemap.CellToLocal(new Vector3Int(cellPos.x + 1, cellPos.y - 1, cellPos.z));
                }) != null;

                hunk.updateSprite(SpriteTileMapping.getMapping(x0y0, x1y0, x2y0, x0y1, x2y1, x0y2, x1y2, x2y2));
            }
        }
    }
}