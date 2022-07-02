using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameControllers.Services;
using Environment.Models;
using Zenject;

namespace Environment
{
    public class GroundLayer : MonoBehaviour2
    {
        private const int MAP_WIDTH = 22;
        private const int MAP_HEIGHT = 12;
        private Tilemap tilemap;
        private TilemapRenderer tilemapRenderer;
        private IEnvironmentService EnvironmentService;
        public Sprite[] spriteList;
        private SpriteRenderer spriteRenderer;


        [Inject]
        public void Construct(IEnvironmentService _EnvironmentService)
        {
            this.tilemap = this.GetComponent<Tilemap>();
            this.tilemapRenderer = this.GetComponent<TilemapRenderer>();
            this.EnvironmentService = _EnvironmentService;
            this.EnvironmentService.groundTiles.Subscribe(groundTiles =>
            {
                this.RefreshTiles(groundTiles);
            });
        }
        // Start is called before the first frame update
        void Start()
        {
            IList<GroundTileModel> newGroundTiles = new List<GroundTileModel>();
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {   
                    newGroundTiles.Add(new GroundTileModel(new Vector3Int(x,y,0), GroundTileModel.eGroundTypes.grass));
                }
            }
            this.EnvironmentService.groundTiles.Set(newGroundTiles);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void RefreshTiles(IList<GroundTileModel> groundTileModels)
        {
            //this.tilemap.ClearAllTiles();
            groundTileModels.ForEach(tile =>
            {
                Vector3Int vec3 = tile.position;
                if (!this.tilemap.HasTile(vec3))
                {
                    this.tilemap.SetTile(vec3, ScriptableObject.CreateInstance<Tile>());
                }
                this.tilemap.GetTile<Tile>(vec3).sprite = this.spriteList[(int)tile.groundType];
            });
            this.tilemap.RefreshAllTiles();
            this.tilemap.CompressBounds();
        }
    }
}
