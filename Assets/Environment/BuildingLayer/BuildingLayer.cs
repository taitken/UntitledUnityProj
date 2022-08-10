
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
        private MouseActionModel mouseAction;
        private BuildingObjectFactory buildingModelFactory;
        private BuildSiteObject.Factory buildSiteFactory;
        private IUiPanelService contextService;
        private IList<BuildingObject> buildingPrefabs = new List<BuildingObject>();
        private IList<BuildingObjectModel> buildingObjectModels
        {
            get
            {
                return this.buildingPrefabs.Map(building => { return building.buildingObjectModel; });
            }
        }
        private IList<BuildSiteObject> buildingSiteObjects = new List<BuildSiteObject>();
        private IList<BuildSiteModel> buildSiteModels
        {
            get
            {
                return this.buildingSiteObjects.Map(buildSite => { return buildSite.buildSiteModel; });
            }
        }

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IEnvironmentService _environmentService,
                              IBuildingService _buildingService,
                              IUiPanelService _contextService,
                              IItemObjectService _itemService,
                              LayerCollider.Factory _layerColliderFactory,
                              BuildSiteObject.Factory _buildSiteFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "BuildingLayer");
            this.orderService = _orderService;
            this.environmentService = _environmentService;
            this.buildingService = _buildingService;
            this.contextService = _contextService;
            this.buildSiteFactory = _buildSiteFactory;
            this.itemService = _itemService;
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
            });
            objsToRemove.ForEach(buildingObj =>
            {
                BuildingObject building = this.buildingPrefabs.Find(building => { return building.buildingObjectModel.ID == buildingObj.ID; });
                this.buildingPrefabs.Remove(building);
                building.Destroy();
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
            BuildingObject building = Instantiate<BuildingObject>(this.buildingService.buildingAssetController.GetBuildingPrefab(buildingObj.buildingType));
            building.Initialise(this.contextService, buildingObj, this.environmentService, this.itemService, this.orderService);
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
            if (this.mouseAction.mouseType == eMouseAction.Build && this.mouseAction.buildingType == eBuildingType.FloorTile)
            {
                Vector3Int startPos = this.environmentService.LocalToCell(new Vector3(dragEvent.initialDragLocation.x + IEnvironmentService.TILE_WIDTH_PIXELS /2, dragEvent.initialDragLocation.y  + IEnvironmentService.TILE_WIDTH_PIXELS /2, 0));
                Vector3Int endPos = this.environmentService.LocalToCell(new Vector3(dragEvent.currentDragLocation.x  + IEnvironmentService.TILE_WIDTH_PIXELS /2, dragEvent.currentDragLocation.y  + IEnvironmentService.TILE_WIDTH_PIXELS /2, 0));
                IList<Vector3Int> draggedCells = this.environmentService.GetCellsInArea(startPos, endPos);
                draggedCells.ForEach(cell => { this.PlaceBuildingBlueprint(cell); });
            }
        }

        public void PlaceBuildingBlueprint(Vector3Int coordinates)
        {
            if ((this.buildingService.IsBuildingSpaceAvailable(coordinates) || this.mouseAction.buildingType == eBuildingType.FloorTile)
            && !this.orderService.IsExistingOrderAtLocation(coordinates)
            && (this.mouseAction.buildingType != eBuildingType.FloorTile || this.buildingService.IsFloorSpaceAvailable(coordinates)))
            {
                BuildingTypeStats.GetBuildingStats(this.mouseAction.buildingType).buildSupply.ForEach(requiredItem =>
                {
                    this.orderService.AddOrder(new BuildSupplyOrderModel(coordinates, requiredItem.itemType, requiredItem.mass, this.mouseAction.buildingType));
                });
            }
        }

        private void ShowBuildGhost(eBuildingType _buildingType)
        {
            BuildingStatsModel buildStats = BuildingTypeStats.GetBuildingStats(_buildingType);
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

    }
}