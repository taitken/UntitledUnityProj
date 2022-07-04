
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Environment.Models;
using GameControllers.Services;
using Zenject;

namespace Environment
{
    public class MineableLayer : MonoBehaviour2
    {
        public MineableHunk mineableHunkPrefab;
        public Tilemap tilemap;
        private IUnitActionService actionService;
        private IEnvironmentService environmentService;
        private MineableHunk.Factory hunkFactory;
        private IList<MineableHunk> mineableHunks = new List<MineableHunk>();

        [Inject]
        public void Construct(IUnitActionService _actionService,
                              MineableHunk.Factory _hunkFactory,
                              IEnvironmentService _environmentService)
        {
            this.hunkFactory = _hunkFactory;
            this.actionService = _actionService;
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
                IList<MineableObjectModel> objsToAdd = mineableObjs.Filter(obj => { return this.mineableHunks.Find(hunk => { return hunk.mineableObjectModel.ID == obj.ID; }) == null; });
                IList<MineableHunk> objsToRemove = this.mineableHunks.Filter(hunk => { return mineableObjs.Find(obj => { return hunk.mineableObjectModel.ID == obj.ID; }) == null; });
                objsToAdd.ForEach(mineableObj =>
                {
                    this.mineableHunks.Add(this.createMineableObject(mineableObj));
                });
                objsToRemove.ForEach(hunk =>
                {
                    mineableHunks.Remove(hunk);
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