
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameControllers.Services;
using Zenject;

namespace Environment
{
    public class MineableTilesLayerController : MonoBehaviour2
    {
        public MineableHunk mineableHunkPrefab;
        public Tilemap tilemap;

        private IUnitActionService actionService;

        private IList<MineableHunk> mineableHunks = new List<MineableHunk>();

        [Inject]
        public void Construct(IUnitActionService _actionService)
        {
            this.actionService = _actionService;
            this.subscriptions.Add(this.actionService.actionQueue.Subscribe(actionQueue =>
            {
                Debug.Log(actionQueue.Count);
            })
            );
        }

        // Start is called before the first frame update
        void Start()
        {
            this.tilemap = GetComponent<Tilemap>();
            for (int x = 0; x < this.tilemap.size.x; x++)
            {
                for (int y = 0; y < this.tilemap.size.y; y++)
                {
                    MineableHunk newHunk = Instantiate(mineableHunkPrefab, this.tilemap.CellToLocal(new Vector3Int(x, y, 0)), Quaternion.identity);
                    mineableHunks.Add(newHunk);
                    newHunk.BeforeDestroy(delegate ()
                    {
                        mineableHunks.Remove(newHunk);
                        this.UpdateTileMap();
                    });
                }
            }
            this.UpdateTileMap();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void UpdateTileMap()
        {
            this.ReMapSpries();
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