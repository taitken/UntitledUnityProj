using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;
using Environment.Models;

namespace Environment
{
    public class GameMapController : MonoBehaviour2
    {

        private const int MAP_WIDTH = 22;
        private const int MAP_HEIGHT = 12;
        public GroundLayer groundLayer;
        public MineableLayer mineableLayer;
        public UnitOrdersLayer unitOrdersLayer;
        private IUnitOrderService orderService;
        private IEnvironmentService environmentService;
        private IPathFinderService pathFinderService;
        private IList<GroundTileModel> groundTiles;
        private IList<MineableObjectModel> mineableObjects;
        private Grid grid;

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IEnvironmentService _environmentService,
                              IPathFinderService _pathFinderService)
        {
            this.orderService = _orderService;
            this.environmentService = _environmentService;
            this.pathFinderService = _pathFinderService;
            this.grid = this.GetComponent<Grid>();
            this.environmentService.tileMapRef = this.groundLayer.GetComponent<Tilemap>();
        }
        // Start is called before the first frame update
        void Start()
        {
            this.ConfigureGroundTiles();
            this.ConfigureMineableTiles();
            this.subscriptions.Add(this.environmentService.groundTiles.Subscribe(groundLayer =>
            {
                this.groundTiles = groundLayer;
                this.ConfigurePathfinderMap();
            }));
            this.subscriptions.Add(this.environmentService.mineableObjects.Subscribe(_mineableObjects =>
            {
                this.mineableObjects = _mineableObjects;
                this.ConfigurePathfinderMap();
            }));
        }

        // Update is called once per frame
        void Update()
        {

        }

        void ConfigureGroundTiles()
        {
            IList<GroundTileModel> newGroundTiles = new List<GroundTileModel>();
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    newGroundTiles.Add(new GroundTileModel(new Vector3Int(x, y, 0), GroundTileModel.eGroundTypes.grass));
                }
            }
            this.environmentService.groundTiles.Set(newGroundTiles);
        }

        void ConfigureMineableTiles()
        {
            IList<MineableObjectModel> newMinableTiles = new List<MineableObjectModel>();
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    if (!(x >= 9 && x <= 13 && y >= 5 && y <= 7))
                    {
                        newMinableTiles.Add(new MineableObjectModel(new Vector3Int(x, y, 0)));
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
                for (int x = 0; x < MAP_WIDTH; x++)
                {
                    IList<PathFinderMapItem> column = new List<PathFinderMapItem>();
                    for (int y = 0; y < MAP_HEIGHT; y++)
                    {
                        column.Add(new PathFinderMapItem(x, y, this.mineableObjects.Find(obj =>{return obj.position.x == x && obj.position.y == y;}) != null));
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