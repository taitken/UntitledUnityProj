
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Environment.Models;
using UnityEngine.InputSystem;
using GameControllers.Services;
using Extensions;
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
        private eMouseAction mouseAction;

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IEnvironmentService _environmentService)
        {
            this.InitiliseMonoLayer();
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
            if (this.mouseAction == eMouseAction.Build)
            {
                this.ShowBuildGhost();
            }
            else
            {
                if(this.ghostBuilding != null) Destroy(this.ghostBuilding);
            }
        }

        private void ShowBuildGhost()
        {
            if (this.ghostBuilding == null)
            {
                this.ghostBuilding = Instantiate(this.buildingGhostPrefab, this.GetCellCoorAtMouse(), new Quaternion());
            }
            else
            {
                this.ghostBuilding.GetComponent<Transform>().position = this.GetCellCoorAtMouse();
            }
        }

    }
}