using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameControllers.Services;
using Characters;
using GameControllers.Models;
using Zenject;
using Environment.Models;

namespace Environment
{
    public class GameSceneController : MonoBehaviour2
    {

        public ActionController actionController;
        public MouseActionController mouseActionController;
        public GameMapController gameMapController;
        public UnitController unitController;
        private IUnitOrderService orderService;
        private IEnvironmentService environmentService;
        private IPathFinderService pathFinderService;

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IEnvironmentService _environmentService,
                              IPathFinderService _pathFinderService)
        {
            this.orderService = _orderService;
            this.environmentService = _environmentService;
            this.pathFinderService = _pathFinderService;
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}