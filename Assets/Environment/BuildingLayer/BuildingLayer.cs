
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using Building.Models;
using Building;
using Zenject;
using GameControllers.Models;
using UI.Services;
using Item.Models;
using UtilityClasses;
using System;

namespace Environment
{
    public class BuildingLayer : MonoBehaviourLayer
    {
        private GameObject ghostBuilding;
        private IUnitOrderService orderService;
        private IBuildingService buildingService;
        private IEnvironmentService environmentService;
        private IItemObjectService itemService;
        private ICropService cropService;
        private MouseActionModel mouseAction;
        private DiContainer diContainer;
        private BuildingObjectFactory buildingModelFactory;
        private BuildSiteObject.Factory buildSiteFactory;
        private IUiPanelService contextService;
        private IList<BuildingObject> buildingPrefabs = new List<BuildingObject>();
        private IList<WallBuildingObject> wallBuildingObjects { get { return this.buildingPrefabs.Filter(building => { return building.buildingObjectModel.buildingType == eBuildingType.Wall; }).Map(wall => { return wall as WallBuildingObject; }); } }
        private IList<BuildingObjectModel> buildingObjectModels { get { return this.buildingPrefabs.Map(building => { return building.buildingObjectModel; }); } }
        private IList<BuildSiteObject> buildingSiteObjects = new List<BuildSiteObject>();
        private IList<BuildSiteModel> buildSiteModels { get { return this.buildingSiteObjects.Map(buildSite => { return buildSite.buildSiteModel; }); } }

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IEnvironmentService _environmentService,
                              IBuildingService _buildingService,
                              IUiPanelService _contextService,
                              IItemObjectService _itemService,
                              ICropService _cropService,
                              LayerCollider.Factory _layerColliderFactory,
                              DiContainer _diContainer,
                              BuildSiteObject.Factory _buildSiteFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "BuildingLayer");
            this.orderService = _orderService;
            this.environmentService = _environmentService;
            this.buildingService = _buildingService;
            this.contextService = _contextService;
            this.buildSiteFactory = _buildSiteFactory;
            this.itemService = _itemService;
            this.cropService = _cropService;
            this.diContainer = _diContainer;
            this.buildingModelFactory = new BuildingObjectFactory();
        }

        void Start()
        {
            this.orderService.mouseAction.Subscribe(this, _mouseAction => { this.mouseAction = _mouseAction; });
            this.buildingService.buildingObseravable.Subscribe(this, this.RefreshBuildings);
            this.buildingService.buildingSiteObseravable.Subscribe(this, this.RefreshBuildSites);
        }
        // Update is called once per frame
        void Update()
        {
            if (this.mouseAction.mouseType == eMouseAction.Build)
            {
                this.ShowBuildGhost(this.mouseAction.buildingType);
            }
            else
            {
                if (this.ghostBuilding != null) Destroy(this.ghostBuilding);
            }
        }

        private void RefreshBuildings(IList<BuildingObjectModel> buildings)
        {
            IList<BuildingObjectModel> objsToAdd = buildings.GetNewModels(this.buildingObjectModels);
            IList<BuildingObjectModel> objsToRemove = buildings.GetRemovedModels(this.buildingObjectModels);
            objsToAdd.ForEach(buildingObj =>
            {
                this.buildingPrefabs.Add(this.CreateBuilding(buildingObj));
                if (buildingObj is WallBuildingModel) this.ReMapBlocksAround(buildingObj.position);
            });
            objsToRemove.ForEach(buildingObj =>
            {
                BuildingObject building = this.buildingPrefabs.Find(building => { return building.buildingObjectModel.ID == buildingObj.ID; });
                this.buildingPrefabs.Remove(building);
                building.Destroy();
                if (buildingObj is WallBuildingModel) this.ReMapBlocksAround(buildingObj.position);
            });
        }

        private void RefreshBuildSites(IList<BuildSiteModel> buildSites)
        {
            IList<BuildSiteModel> objsToAdd = buildSites.GetNewModels(this.buildSiteModels);
            IList<BuildSiteModel> objsToRemove = buildSites.GetRemovedModels(this.buildSiteModels);
            objsToAdd.ForEach(buildSite =>
            {
                this.buildingSiteObjects.Add(this.CreateBuildSite(buildSite));
            });
            objsToRemove.ForEach(buildingObj =>
            {
                BuildSiteObject buildSiteToRemove = this.buildingSiteObjects.Find(buildSite => { return buildSite.buildSiteModel.ID == buildingObj.ID; });
                buildingSiteObjects.Remove(buildSiteToRemove);
                buildSiteToRemove.Destroy();
            });
        }

        private BuildingObject CreateBuilding(BuildingObjectModel buildingObj)
        {
            BuildingObject building = this.diContainer.InstantiatePrefab(this.buildingService.buildingAssetController.GetBuildingPrefab(buildingObj.buildingType)).GetComponent<BuildingObject>();
            building.Initialise(this.contextService, buildingObj, this.environmentService, this.itemService, this.buildingService, this.orderService, this.cropService);
            return building;
        }

        private BuildSiteObject CreateBuildSite(BuildSiteModel buildSiteModel)
        {
            BuildSiteObject buildSite = this.buildSiteFactory.Create(buildSiteModel);
            buildSite.transform.position = this.tilemap.CellToLocal(buildSiteModel.position);
            return buildSite;
        }

        private void RemoveBuildings()
        {

        }

        public override void OnMouseOver()
        {

        }

        public override void OnClickedByUser()
        {
            if (this.mouseAction.mouseType == eMouseAction.Build)
            {
                this.PlaceBuildingBlueprint(this.GetCellCoorAtMouse());
            }
        }

        public override void OnDragEnd(DragEventModel dragEvent)
        {
            eBuildingCategory buildingCategory = this.mouseAction.buildingType != eBuildingType.none ? BuildingStatsLibrary.GetBuildingStats(this.mouseAction.buildingType).buildCategory : eBuildingCategory.None;
            if (this.mouseAction.mouseType == eMouseAction.Build &&
                (buildingCategory  == eBuildingCategory.FloorTile || buildingCategory == eBuildingCategory.Grower))
            {
                this.HandleAreaBuild(dragEvent);
            }
            if (this.mouseAction.mouseType == eMouseAction.Build && buildingCategory == eBuildingCategory.Wall)
            {
                this.HandleWallDragEnd(dragEvent);
            }
        }

        private void HandleAreaBuild(DragEventModel dragEvent)
        {
            Vector3Int startPos = this.environmentService.LocalToCell(new Vector3(dragEvent.initialDragLocation.x + IEnvironmentService.TILE_WIDTH_PIXELS / 2, dragEvent.initialDragLocation.y + IEnvironmentService.TILE_WIDTH_PIXELS / 2, 0));
            Vector3Int endPos = this.environmentService.LocalToCell(new Vector3(dragEvent.currentDragLocation.x + IEnvironmentService.TILE_WIDTH_PIXELS / 2, dragEvent.currentDragLocation.y + IEnvironmentService.TILE_WIDTH_PIXELS / 2, 0));
            IList<Vector3Int> draggedCells = this.environmentService.GetCellsInArea(startPos, endPos);
            draggedCells.ForEach(cell => { this.PlaceBuildingBlueprint(cell); });
        }

        private void HandleWallDragEnd(DragEventModel dragEvent)
        {
            Vector3Int startPos = this.environmentService.LocalToCell(new Vector3(dragEvent.initialDragLocation.x + IEnvironmentService.TILE_WIDTH_PIXELS / 2, dragEvent.initialDragLocation.y + IEnvironmentService.TILE_WIDTH_PIXELS / 2, 0));
            Vector3Int endPos = this.environmentService.LocalToCell(new Vector3(dragEvent.currentDragLocation.x + IEnvironmentService.TILE_WIDTH_PIXELS / 2, dragEvent.currentDragLocation.y + IEnvironmentService.TILE_WIDTH_PIXELS / 2, 0));
            IList<Vector3Int> draggedCells = this.environmentService.GetCellsInArea(startPos, endPos);
            draggedCells.ForEach(cell =>
            {
                if (startPos.x == cell.x || startPos.y == cell.y
                    || endPos.x == cell.x || endPos.y == cell.y)
                {
                    this.PlaceBuildingBlueprint(cell);
                }
            });

        }

        public void PlaceBuildingBlueprint(Vector3Int coordinates)
        {
            eBuildingCategory buildingCategory = BuildingStatsLibrary.GetBuildingStats(this.mouseAction.buildingType).buildCategory;
            if ((this.buildingService.IsBuildingSpaceAvailable(coordinates) || buildingCategory == eBuildingCategory.FloorTile)
            && !this.orderService.IsExistingOrderAtLocation(coordinates)
            && (buildingCategory != eBuildingCategory.FloorTile || this.buildingService.IsFloorSpaceAvailable(coordinates)))
            {
                BuildingStatsLibrary.GetBuildingStats(this.mouseAction.buildingType).buildSupply.ForEach(requiredItem =>
                {
                    this.orderService.AddOrder(new BuildSupplyOrderModel(coordinates, requiredItem.itemType, requiredItem.mass, this.mouseAction.buildingType));
                });
            }
        }

        private void ShowBuildGhost(eBuildingType _buildingType)
        {
            BuildingStatsModel buildStats = BuildingStatsLibrary.GetBuildingStats(_buildingType);
            if (this.ghostBuilding == null)
            {
                this.ghostBuilding = this.buildingService.GetBuildingGhostPrefab(_buildingType);
            }
            else
            {
                SpriteRenderer sr = this.ghostBuilding.GetComponent<SpriteRenderer>();
                this.ghostBuilding.transform.position = this.GetLocalPositionOfCellAtMouse() + new Vector3(sr.bounds.size.x / 2, sr.bounds.size.y / 2)
                - new Vector3(IEnvironmentService.TILE_WIDTH_PIXELS / 2, IEnvironmentService.TILE_WIDTH_PIXELS / 2);
            }
        }

        private void ReMapBlocksAround(Vector3Int centerPoint)
        {
            WallBuildingObject[,] buildingMap = new WallBuildingObject[MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT];
            this.wallBuildingObjects.ForEach(wall =>
            {
                buildingMap[wall.wallBuildingModel.position.x, wall.wallBuildingModel.position.y] = wall;
            });
            WallBuildingObject[,] hunksToRefresh = new WallBuildingObject[3, 3];
            hunksToRefresh[0, 0] = buildingMap.ValidIndex(centerPoint.x - 1, centerPoint.y - 1) ? buildingMap[centerPoint.x - 1, centerPoint.y - 1] : null;
            hunksToRefresh[0, 1] = buildingMap.ValidIndex(centerPoint.x - 1, centerPoint.y) ? buildingMap[centerPoint.x - 1, centerPoint.y] : null;
            hunksToRefresh[0, 2] = buildingMap.ValidIndex(centerPoint.x - 1, centerPoint.y + 1) ? buildingMap[centerPoint.x - 1, centerPoint.y + 1] : null;
            hunksToRefresh[1, 0] = buildingMap.ValidIndex(centerPoint.x, centerPoint.y - 1) ? buildingMap[centerPoint.x, centerPoint.y - 1] : null;
            hunksToRefresh[1, 1] = buildingMap.ValidIndex(centerPoint.x, centerPoint.y) ? buildingMap[centerPoint.x, centerPoint.y] : null;
            hunksToRefresh[1, 2] = buildingMap.ValidIndex(centerPoint.x, centerPoint.y + 1) ? buildingMap[centerPoint.x, centerPoint.y + 1] : null;
            hunksToRefresh[2, 0] = buildingMap.ValidIndex(centerPoint.x + 1, centerPoint.y - 1) ? buildingMap[centerPoint.x + 1, centerPoint.y - 1] : null;
            hunksToRefresh[2, 1] = buildingMap.ValidIndex(centerPoint.x + 1, centerPoint.y) ? buildingMap[centerPoint.x + 1, centerPoint.y] : null;
            hunksToRefresh[2, 2] = buildingMap.ValidIndex(centerPoint.x + 1, centerPoint.y + 1) ? buildingMap[centerPoint.x + 1, centerPoint.y + 1] : null;
            this.ReMapSprites(hunksToRefresh);
        }

        private void ReMapSprites(WallBuildingObject[,] wallsToRefresh)
        {
            WallBuildingObject[,] buildingMap = new WallBuildingObject[MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT];
            this.wallBuildingObjects.ForEach(wall =>
            {
                buildingMap[wall.wallBuildingModel.position.x, wall.wallBuildingModel.position.y] = wall;
            });
            // Very inefficient implementation
            // -- To redo
            foreach (WallBuildingObject wall in wallsToRefresh)
            {
                if (wall != null)
                {
                    Vector3Int cellPos = this.tilemap.LocalToCell(wall.gameObject.transform.localPosition);
                    bool x0y0 = SpriteTileMapping.HunkExistsInPosition(cellPos.x - 1, cellPos.y + 1, buildingMap);
                    bool x1y0 = SpriteTileMapping.HunkExistsInPosition(cellPos.x, cellPos.y + 1, buildingMap);
                    bool x2y0 = SpriteTileMapping.HunkExistsInPosition(cellPos.x + 1, cellPos.y + 1, buildingMap);
                    bool x0y1 = SpriteTileMapping.HunkExistsInPosition(cellPos.x - 1, cellPos.y, buildingMap);
                    bool x2y1 = SpriteTileMapping.HunkExistsInPosition(cellPos.x + 1, cellPos.y, buildingMap);
                    bool x0y2 = SpriteTileMapping.HunkExistsInPosition(cellPos.x - 1, cellPos.y - 1, buildingMap);
                    bool x1y2 = SpriteTileMapping.HunkExistsInPosition(cellPos.x, cellPos.y - 1, buildingMap);
                    bool x2y2 = SpriteTileMapping.HunkExistsInPosition(cellPos.x + 1, cellPos.y - 1, buildingMap);
                    wall.UpdateSprite(SpriteTileMapping.getMapping(x0y0, x1y0, x2y0, x0y1, x2y1, x0y2, x1y2, x2y2));
                }
            }
        }


    }
}