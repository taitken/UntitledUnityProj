
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

namespace Environment
{
    public class BuildingLayer : MonoBehaviourLayer
    {
        private GameObject ghostBuilding;
        private IUnitOrderService orderService;
        private IBuildingService buildingService;
        private IEnvironmentService environmentService;
        private MouseActionModel mouseAction;
        private BuildingObjectFactory buildingModelFactory;
        private BuildSiteObject.Factory buildSiteFactory;
        private IContextWindowService contextService;
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
                              IContextWindowService _contextService,
                              LayerCollider.Factory _layerColliderFactory,
                              BuildSiteObject.Factory _buildSiteFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "BuildingLayer");
            this.orderService = _orderService;
            this.environmentService = _environmentService;
            this.buildingService = _buildingService;
            this.contextService = _contextService;
            this.buildSiteFactory = _buildSiteFactory;
            this.buildingModelFactory = new BuildingObjectFactory();
        }

        void Start()
        {
            this.orderService.mouseAction.Subscribe(_mouseAction =>
            {
                this.mouseAction = _mouseAction;
            });
            this.buildingService.buildingObseravable.Subscribe(buildings =>
            {
                this.RefreshBuildings(buildings);
            });
            this.buildingService.buildingSiteObseravable.Subscribe(buildSites =>
            {
                this.RefreshBuildSites(buildSites);
            });
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
            building.Initialise(this.contextService, buildingObj, this.environmentService);
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
                if ((this.buildingService.IsBuildingSpaceAvailable(this.GetCellCoorAtMouse()) || this.mouseAction.buildingType == eBuildingType.FloorTile)
                && !this.orderService.IsExistingOrderAtLocation(this.GetCellCoorAtMouse())
                && (this.mouseAction.buildingType != eBuildingType.FloorTile || this.buildingService.IsFloorSpaceAvailable(this.GetCellCoorAtMouse())))
                {
                    this.buildingModelFactory.CreateBuildingModel(this.GetCellCoorAtMouse(), this.mouseAction.buildingType).requiredItems.ForEach(requiredItem =>
                    {
                        this.orderService.AddOrder(new SupplyOrderModel(this.GetCellCoorAtMouse(), requiredItem.itemType, requiredItem.mass, this.mouseAction.buildingType));
                    });
                }
            }
        }

        private void ShowBuildGhost(eBuildingType _buildingType)
        {
            if (this.ghostBuilding == null)
            {
                this.ghostBuilding = Instantiate(this.buildingService.GetBuildingPrefab(_buildingType).gameObject, this.GetLocalPositionOfCellAtMouse(), new Quaternion());
                SpriteRenderer sr = this.ghostBuilding.GetComponent<SpriteRenderer>();
                sr.sortingOrder = 500;
                sr.color = GameColors.AddTransparency(sr.color, 0.6f);
                this.ghostBuilding.layer = 0;
            }
            else
            {
                this.ghostBuilding.GetComponent<Transform>().position = this.GetLocalPositionOfCellAtMouse();
            }
        }

    }
}