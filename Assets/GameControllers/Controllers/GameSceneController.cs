
using UnityEngine;
using GameControllers.Services;
using GameControllers;
using Zenject;

namespace GameControllers
{
    public class GameSceneController : MonoBehaviour2
    {

        public ActionController actionController;
        public MouseActionController mouseActionController;
        public GameMapController gameMapController;
        private IUnitOrderService orderService;
        private IEnvironmentService environmentService;
        private IBuildingService buildingService;
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