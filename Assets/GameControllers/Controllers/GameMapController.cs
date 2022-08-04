using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;
using Environment.Models;
using Unit.Models;
using Item;
using Characters;
using Environment;
using MineableBlocks.Models;

namespace GameControllers
{
    public class GameMapController : MonoBehaviour2
    {
        public GroundLayer groundLayer;
        public MineableLayer mineableLayer;
        public ItemObjectLayer itemObjectLayer;
        public UnitOrdersLayer unitOrdersLayer;
        public BuildingLayer buildingLayer;
        public CharacterLayer characterLayer;
        private IUnitOrderService orderService;
        private IEnvironmentService environmentService;
        private IPathFinderService pathFinderService;
        private IItemObjectService itemObjectService;
        private IList<GroundTileModel> groundTiles;
        private IList<MineableObjectModel> mineableObjects;
        private Grid grid;

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IEnvironmentService _environmentService,
                              IPathFinderService _pathFinderService,
                              IItemObjectService _itemObjectService)
        {
            this.orderService = _orderService;
            this.environmentService = _environmentService;
            this.pathFinderService = _pathFinderService;
            this.itemObjectService = _itemObjectService;
            this.grid = this.GetComponent<Grid>();
            this.groundLayer = this.GetComponentInChildren<GroundLayer>();
            this.mineableLayer = this.GetComponentInChildren<MineableLayer>();
            this.itemObjectLayer = this.GetComponentInChildren<ItemObjectLayer>();
            this.unitOrdersLayer = this.GetComponentInChildren<UnitOrdersLayer>();
            this.buildingLayer = this.GetComponentInChildren<BuildingLayer>();
            this.characterLayer = this.GetComponentInChildren<CharacterLayer>();
            this.environmentService.tileMapRef = this.groundLayer.GetComponent<Tilemap>();
        }
        // Start is called before the first frame update
        void Start()
        {
            this.ConfigureGroundTiles();
            this.ConfigureMineableTiles();
            this.environmentService.groundTiles.Subscribe(this, groundLayer =>
            {
                this.groundTiles = groundLayer;
                this.ConfigurePathfinderMap();
            });
            this.environmentService.mineableObjects.Subscribe(this, _mineableObjects =>
            {
                this.mineableObjects = _mineableObjects;
                this.ConfigurePathfinderMap();
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        void ConfigureGroundTiles()
        {
            IList<GroundTileModel> newGroundTiles = new List<GroundTileModel>();
            for (int x = 0; x < MonoBehaviourLayer.MAP_WIDTH; x++)
            {
                for (int y = 0; y < MonoBehaviourLayer.MAP_HEIGHT; y++)
                {
                    newGroundTiles.Add(new GroundTileModel(new Vector3Int(x, y, 0), Random.Range(200, 400), GroundTileModel.eGroundTypes.grass));
                }
            }
            this.environmentService.groundTiles.Set(newGroundTiles);
        }

        void ConfigureMineableTiles()
        {
            IList<MineableObjectModel> newMinableTiles = new List<MineableObjectModel>();
            for (int x = 0; x < MonoBehaviourLayer.MAP_WIDTH; x++)
            {
                for (int y = 0; y < MonoBehaviourLayer.MAP_HEIGHT; y++)
                {
                    if (!(x >= 9 && x <= 13 && y >= 5 && y <= 7))
                    {
                        if (x >= 15 && x <= 17 && y >= 5 && y <= 7)
                        {
                            newMinableTiles.Add(new MineableObjectModel(new Vector3Int(x, y, 0), eMineableBlockType.Coal, Random.Range(800, 1200), MineableBlockTypeStats.GetMineableBlockStats(eMineableBlockType.Coal)));
                        }
                        else
                        {
                            newMinableTiles.Add(new MineableObjectModel(new Vector3Int(x, y, 0), eMineableBlockType.Stone, Random.Range(200, 400), MineableBlockTypeStats.GetMineableBlockStats(eMineableBlockType.Stone)));
                        }
                    }
                }
            }
            this.environmentService.mineableObjects.Set(newMinableTiles);
        }


        void ConfigurePathfinderMap()
        {
            if (this.mineableObjects != null && this.groundLayer != null)
            {
                IList<IList<PathFinderMapItem>> newMap = new List<IList<PathFinderMapItem>>();
                for (int x = 0; x < MonoBehaviourLayer.MAP_WIDTH; x++)
                {
                    IList<PathFinderMapItem> column = new List<PathFinderMapItem>();
                    for (int y = 0; y < MonoBehaviourLayer.MAP_HEIGHT; y++)
                    {
                        column.Add(new PathFinderMapItem(x, y, this.mineableObjects.Find(obj => { return obj.position.x == x && obj.position.y == y; }) != null));
                    }
                    newMap.Add(column);
                }
                this.pathFinderService.pathFinderMap.Set(new PathFinderMap(newMap));
            }
        }



        public override void OnClickedByUser()
        {
            Debug.Log("clicked");
        }
    }
}