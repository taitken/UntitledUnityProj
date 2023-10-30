using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;
using Environment.Models;
using Environment;
using MineableBlocks.Models;
using Building.Models;
using Item.Models;
using static Item.Models.ItemObjectModel;

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
        private IBuildingService buildingService;
        private IList<GroundTileModel> groundTiles;
        private Grid grid;

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IEnvironmentService _environmentService,
                              IPathFinderService _pathFinderService,
                              IItemObjectService _itemObjectService,
                              IBuildingService _buildingService)
        {
            this.orderService = _orderService;
            this.environmentService = _environmentService;
            this.pathFinderService = _pathFinderService;
            this.itemObjectService = _itemObjectService;
            this.buildingService = _buildingService;
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
            this.ConfigureFogObjects();
            this.environmentService.groundTiles.Subscribe(this, groundLayer =>
            {
                this.groundTiles = groundLayer;
            });
            this.environmentService.mineableObjects.Subscribe(this, _mineableObjects => { this.ConfigurePathfinderMap(); });
            this.buildingService.buildingObseravable.Subscribe(this, _buildings => { this.ConfigurePathfinderMap(); });
            this.ConfigureStartingItems();
            this.CreateRoom(new Vector3Int(45, 45), new Vector3Int(50, 55), new Vector3Int(50, 50), eFloorType.Barracks);
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
                    newGroundTiles.Add(new GroundTileModel(new Vector3Int(x, y, 0), new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, Random.Range(200, 400)) }, GroundTileModel.eGroundTypes.grass));
                }
            }
            this.environmentService.groundTiles.Set(newGroundTiles);
        }

        private void ConfigureMineableTiles()
        {
            MineableObjectModel[,] newmineableTiles = new MineableObjectModel[MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT];
            this.SetBlockDeposit(newmineableTiles, eMineableBlockType.Coal, 25, 35, 70, 20);
            this.SetBlockDeposit(newmineableTiles, eMineableBlockType.Copper, 30, 35, 60, 35);
            this.FillMapGapsWithStoneBlocks(newmineableTiles);
            this.CompleteMap(newmineableTiles);
        }

        private void ConfigureFogObjects()
        {
            FogModel[,] newFogModels = new FogModel[MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT];
            for (int i = 0; i < newFogModels.GetLength(0); i++)
            {
                for (int ii = 0; ii < newFogModels.GetLength(1); ii++)
                {
                    if (!this.IsStartingZone(i, ii, 2, 2))
                    {
                        newFogModels[i, ii] = new FogModel(new Vector3Int(i, ii), new List<ItemObjectMass>());
                    }
                }
            }
            this.environmentService.GetFogObservable().Set(newFogModels);
        }

        private void ConfigureStartingItems()
        {
            StorageBuildingModel newChest = new StorageBuildingModel(new Vector3Int(47, 47), eBuildingType.Chest, BuildingStatsLibrary.GetBuildingStats(eBuildingType.Chest));
            this.buildingService.AddBuilding(newChest);
            IList<ItemObjectModel> objectsToStore = new List<ItemObjectModel>();
            objectsToStore.Add(new ItemObjectModel(newChest.position, new ItemObjectMass(eItemType.GrunberrySeed, 1), eItemState.InStorage, false));
            objectsToStore.Add(new ItemObjectModel(newChest.position, new ItemObjectMass(eItemType.BlumberrySeed, 1), eItemState.InStorage, false));
            objectsToStore.Add(new ItemObjectModel(newChest.position, new ItemObjectMass(eItemType.LuttipodSeed, 1), eItemState.InStorage, false));
            objectsToStore.Add(new ItemObjectModel(newChest.position, new ItemObjectMass(eItemType.PubberbillSeed, 1), eItemState.InStorage, false));
            objectsToStore.Add(new ItemObjectModel(newChest.position, new ItemObjectMass(eItemType.Stone, 10000), eItemState.InStorage, false));
            objectsToStore.ForEach(obj =>
            {
                this.itemObjectService.AddItemToWorld(obj);
                newChest.StoreItem(obj);
            });
        }

        private void CreateRoom(Vector3Int _bottomLeftCorner, Vector3Int _topRightCorner, Vector3Int doorLocation, eFloorType _floorType)
        {
            // Walls
            IList<WallBuildingModel> walls = new List<WallBuildingModel>();
            walls.AddRange(this.BuildWall(_bottomLeftCorner, new Vector3Int(_bottomLeftCorner.x, _topRightCorner.y), true));
            walls.AddRange(this.BuildWall(new Vector3Int(_bottomLeftCorner.x + 1, _bottomLeftCorner.y), new Vector3Int(_topRightCorner.x, _bottomLeftCorner.y), false));
            walls.AddRange(this.BuildWall(new Vector3Int(_bottomLeftCorner.x, _topRightCorner.y), _topRightCorner, false));
            walls.AddRange(this.BuildWall(new Vector3Int(_topRightCorner.x, _bottomLeftCorner.y), new Vector3Int(_topRightCorner.x, _topRightCorner.y + 1), true));

            walls.ForEach(wall =>
            {
                if (wall.position != doorLocation)
                    this.buildingService.AddBuilding(wall);
            });
            
            // Door
            this.buildingService.AddBuilding(new DoorBuildingModel(doorLocation, eBuildingType.Door, BuildingStatsLibrary.GetBuildingStats(eBuildingType.Door)));

            // Floor
            BuildingStatsModel floorType = BuildingStatsLibrary.GetBuildingStats().Find(stats => { return stats?.floorType == _floorType; });
            for (int i = 1; i < _topRightCorner.x - _bottomLeftCorner.x; i++)
            {
                for (int ii = 1; ii < _topRightCorner.y - _bottomLeftCorner.y; ii++)
                {
                    this.buildingService.AddBuilding(new FloorTileModel(new Vector3Int(_bottomLeftCorner.x + i, _bottomLeftCorner.y + ii), floorType.buildingType, floorType));
                }
            }
        }

        private IList<WallBuildingModel> BuildWall(Vector3Int startOfLine, Vector3Int endOfLine, bool buildVertical)
        {
            IList<WallBuildingModel> walls = new List<WallBuildingModel>();
            int stepCount = buildVertical ? endOfLine.y - startOfLine.y : endOfLine.x - startOfLine.x;
            for (int i = 0; i < stepCount; i++)
            {
                Vector3Int pos = buildVertical ? new Vector3Int(startOfLine.x, startOfLine.y + i) : new Vector3Int(startOfLine.x + i, startOfLine.y);
                walls.Add(new WallBuildingModel(pos, eBuildingType.Wall, BuildingStatsLibrary.GetBuildingStats(eBuildingType.Wall)));
            }
            return walls;
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
            return this.IsStartingZone(x, y, 0, 0);
        }

        private bool IsStartingZone(int x, int y, int extraX, int extraY)
        {
            return (x >= 45 - extraX && x <= 55 + extraX && y >= 45 - extraY && y <= 55 + extraY);
        }

        private void CompleteMap(MineableObjectModel[,] newMinableTiles)
        {
            this.environmentService.mineableObjects.Set(newMinableTiles);
        }

        private void ConfigurePathfinderMap()
        {
            var _mineableObjects = this.environmentService.mineableObjects.Get();
            BuildingObjectModel[,] _walls = new BuildingObjectModel[MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT];
            this.buildingService.buildingObseravable.Get().Filter(building => { return building is WallBuildingModel; }).ForEach(wall =>
            {
                _walls[wall.position.x, wall.position.y] = wall;
            });
            if (_mineableObjects != null && this.groundLayer != null)
            {
                PathFinderMapItem[,] newMap = new PathFinderMapItem[MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT];
                for (int x = 0; x < newMap.GetLength(0); x++)
                {
                    for (int y = 0; y < newMap.GetLength(1); y++)
                    {
                        newMap[x, y] = new PathFinderMapItem(x, y, _mineableObjects[x, y] != null || _walls[x, y] != null);
                    }
                }
                this.pathFinderService.SetPathFinderMap(new PathFinderMap(newMap));
            }
        }

        public override void OnClickedByUser()
        {
            Debug.Log("clicked");
        }
    }
}