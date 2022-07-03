using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;
using Environment.Models;

namespace Environment
{
    public class GameMapController : MonoBehaviour2
    {

        private const int MAP_WIDTH = 22;
        private const int MAP_HEIGHT = 12;
        public GroundLayer groundLayer;
        public MineableLayer mineableLayer;
        public UnitOrdersLayer unitOrdersLayer;
        private IUnitActionService actionService;
        private IEnvironmentService environmentService;
        private IPathFinderService pathFinderService;
        private Grid grid;

        [Inject]
        public void Construct(IUnitActionService _actionService,
                              IEnvironmentService _environmentService,
                              IPathFinderService _pathFinderService)
        {
            this.actionService = _actionService;
            this.environmentService = _environmentService;
            this.pathFinderService = _pathFinderService;
            this.grid = this.GetComponent<Grid>();
        }
        // Start is called before the first frame update
        void Start()
        {
            IList<GroundTileModel> newGroundTiles = new List<GroundTileModel>();
            IList<IList<PathFinderMapItem>> newMap = new List<IList<PathFinderMapItem>>();
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                IList<PathFinderMapItem> column = new List<PathFinderMapItem>();
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    newGroundTiles.Add(new GroundTileModel(new Vector3Int(x, y, 0), GroundTileModel.eGroundTypes.grass));
                    column.Add(new PathFinderMapItem(x, y));
                }
                newMap.Add(column);
            }
            this.environmentService.groundTiles.Set(newGroundTiles);
            this.pathFinderService.pathFinderMap.mapitems = newMap;
            IList<Vector3Int> path = this.pathFinderService.FindPath(new Vector3Int(12, 8), -new Vector3Int(5, 8), this.pathFinderService.pathFinderMap);
            path.ForEach(item =>
            {
                Debug.Log(item);
            });
            IList<Vector3Int> secondPath = this.pathFinderService.FindPath(new Vector3Int(12, 8), -new Vector3Int(16, 8), this.pathFinderService.pathFinderMap);
            secondPath.ForEach(item =>
            {
                Debug.Log(item);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnClickedByUser()
        {
            Debug.Log("clicked");
        }
    }
}