
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using Building.Models;
using Building;
using Zenject;
using GameControllers.Models;

namespace Environment
{
    public class BuildingLayer : MonoBehaviourLayer
    {
        public GameObject buildingGhostPrefab;
        private GameObject ghostBuilding;
        private IUnitOrderService orderService;
        private IBuildingService buildingService;
        private IEnvironmentService environmentService;
        private MouseActionModel mouseAction;
        private IList<BuildingObject> buildingPrefabs = new List<BuildingObject>();
        private IList<BuildingObjectModel> buildingObjectModels
        {
            get
            {
                return this.buildingPrefabs.Map(building => { return building.buildingObjectModel; });
            }
        }

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IEnvironmentService _environmentService,
                              IBuildingService _buildingService,
                              LayerCollider.Factory _layerColliderFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "BuildingLayer");
            this.orderService = _orderService;
            this.environmentService = _environmentService;
            this.buildingService = _buildingService;

        }

        void Start()
        {
            this.orderService.mouseAction.Subscribe(_mouseAction =>
            {
                this.mouseAction = _mouseAction;
            });
            this.buildingService.buildingSubscribable.Subscribe(buildings =>
            {
                this.RefreshBuildings(buildings);
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

        private BuildingObject CreateBuilding(BuildingObjectModel buildingObj)
        {
            BuildingObject building = Instantiate<BuildingObject>(this.buildingService.buildingAssetController.GetBuildingPrefab(buildingObj.buildingType)); 
            building.Initialise(buildingObj, this.tilemap);
            return building;
        }

        private void RemoveBuildings()
        {

        }

        public override void OnMouseOver()
        {
            Debug.Log("test");
        }

        public override void OnClickedByUser()
        {
            Debug.Log(this.GetCellCoorAtMouse());
            this.orderService.AddOrder(new BuildOrderModel(this.GetCellCoorAtMouse(), this.mouseAction.buildingType));
        }

        private void ShowBuildGhost(eBuildingType _buildingType)
        {
            if (this.ghostBuilding == null)
            {
                this.ghostBuilding = Instantiate(this.buildingGhostPrefab, this.GetLocalPositionOfCellAtMouse(), new Quaternion());
            }
            else
            {
                this.ghostBuilding.GetComponent<Transform>().position = this.GetLocalPositionOfCellAtMouse();
            }
        }

    }
}