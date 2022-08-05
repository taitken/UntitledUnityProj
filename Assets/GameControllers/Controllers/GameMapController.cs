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
                this.ConfigurePathfinderMap();
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ConfigureGroundTiles()
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

        private void ConfigureMineableTiles()
        {
            MineableObjectModel[,] newmineableTiles = new MineableObjectModel[MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT];
            this.SetBlockDeposit(newmineableTiles, eMineableBlockType.Coal, 7, 11, 70, 20);
            this.SetBlockDeposit(newmineableTiles, eMineableBlockType.Copper, 7, 8, 60, 35);
            this.FillMapGapsWithStoneBlocks(newmineableTiles);
            this.CompleteMap(newmineableTiles);
        }

        private void SetBlockDeposit(MineableObjectModel[,] newMineableTiles, eMineableBlockType blockType, int depositMin, int depositMax, int spreadChance, int spreadDecrement)
        {
            int depositCount = Random.Range(depositMin, depositMax);
            for (int i = 0; i < depositCount; i++)
            {
                Vector3Int position = new Vector3Int(Random.Range(0, newMineableTiles.GetLength(0) - 1), Random.Range(0, newMineableTiles.GetLength(1) - 1));
                this.ExpandBlockDeposit(newMineableTiles, position, blockType, spreadChance, spreadDecrement);
            }
        }

        private void FillMapGapsWithStoneBlocks(MineableObjectModel[,] newMineableTiles)
        {
            for (int x = 0; x < newMineableTiles.GetLength(0); x++)
            {
                for (int y = 0; y < newMineableTiles.GetLength(1); y++)
                {
                    if (!this.IsStartingZone(x, y) && newMineableTiles[x, y] == null)
                    {
                        newMineableTiles[x, y] = new MineableObjectModel(new Vector3Int(x, y, 0), eMineableBlockType.Stone, MineableBlockTypeStats.GetMineableBlockStats(eMineableBlockType.Stone));
                    }
                }
            }
        }

        private void ExpandBlockDeposit(MineableObjectModel[,] minaebleTiles, Vector3Int location, eMineableBlockType type, int percentageChance, int chanceDecrement)
        {
            if (minaebleTiles.ValidIndex(location.x, location.y) && minaebleTiles[location.x, location.y] == null && !this.IsStartingZone(location.x, location.y))
            {
                minaebleTiles[location.x, location.y] = new MineableObjectModel(new Vector3Int(location.x, location.y, 0), type, MineableBlockTypeStats.GetMineableBlockStats(type));
                if (Random.Range(1, 100) <= percentageChance) this.ExpandBlockDeposit(minaebleTiles, location + new Vector3Int(0, 1), type, percentageChance - chanceDecrement, chanceDecrement);
                if (Random.Range(1, 100) <= percentageChance) this.ExpandBlockDeposit(minaebleTiles, location + new Vector3Int(1, 0), type, percentageChance - chanceDecrement, chanceDecrement);
                if (Random.Range(1, 100) <= percentageChance) this.ExpandBlockDeposit(minaebleTiles, location + new Vector3Int(0, -1), type, percentageChance - chanceDecrement, chanceDecrement);
                if (Random.Range(1, 100) <= percentageChance) this.ExpandBlockDeposit(minaebleTiles, location + new Vector3Int(-1, 0), type, percentageChance - chanceDecrement, chanceDecrement);
            }

        }

        private bool IsStartingZone(int x, int y)
        {
            return (x >= 9 && x <= 13 && y >= 5 && y <= 7);
        }

        private void CompleteMap(MineableObjectModel[,] newMinableTiles)
        {
            this.environmentService.mineableObjects.Set(newMinableTiles);
        }

        private void ConfigurePathfinderMap()
        {
            var _mineableObjects = this.environmentService.mineableObjects.Get();
            if (_mineableObjects != null && this.groundLayer != null)
            {
                IList<IList<PathFinderMapItem>> newMap = new List<IList<PathFinderMapItem>>();
                for (int x = 0; x < MonoBehaviourLayer.MAP_WIDTH; x++)
                {
                    IList<PathFinderMapItem> column = new List<PathFinderMapItem>();
                    for (int y = 0; y < MonoBehaviourLayer.MAP_HEIGHT; y++)
                    {
                        column.Add(new PathFinderMapItem(x, y, _mineableObjects[x, y] != null));
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