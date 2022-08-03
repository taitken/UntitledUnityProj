using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameControllers.Services;
using Environment.Models;
using Zenject;
using UnityEngine.InputSystem;

namespace Environment
{
    public class GroundLayer : MonoBehaviourLayer
    {
        private TilemapRenderer tilemapRenderer;
        private IEnvironmentService environmentService;
        public Sprite[] spriteList;
        private SpriteRenderer spriteRenderer;


        [Inject]
        public void Construct(IEnvironmentService _environmentService,
                              LayerCollider.Factory _layerColliderFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "GroundLayer");
            this.tilemap = this.GetComponent<Tilemap>();
            this.tilemapRenderer = this.GetComponent<TilemapRenderer>();
            this.environmentService = _environmentService;
            this.environmentService.groundTiles.Subscribe(this, groundTiles => { this.RefreshTiles(groundTiles); });
        }
        // Start is called before the first frame update
        void Start()
        {

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
