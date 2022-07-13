
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using Building.Models;
using Zenject;
using GameControllers.Models;

namespace Environment
{
    public class BuildingLayer : MonoBehaviourLayer
    {
        public GameObject buildingGhostPrefab;
        private GameObject ghostBuilding;
        private IUnitOrderService orderService;
        private IEnvironmentService environmentService;
        private MouseActionModel mouseAction;

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IEnvironmentService _environmentService,
                              LayerCollider.Factory _layerColliderFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "BuildingLayer");
            this.orderService = _orderService;
            this.environmentService = _environmentService;
            this.orderService.mouseAction.Subscribe(_mouseAction =>
            {
                this.mouseAction = _mouseAction;
            });
        }

        // Start is called before the first frame update
        void Start()
        {

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