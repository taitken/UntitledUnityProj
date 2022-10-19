
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Environment.Models;
using GameControllers.Services;
using Extensions;
using Zenject;
using Building;
using Building.Models;

namespace Environment
{
    public class FogLayer : MonoBehaviourLayer
    {
        private IUnitOrderService orderService;
        private IEnvironmentService environmentService;
        private IBuildingService buildingService;
        private FogObject.Factory fogFactory;
        private FogObject[,] fogObjects;

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              FogObject.Factory _fogFactory,
                              IEnvironmentService _environmentService,
                              IBuildingService _buildingService,
                              LayerCollider.Factory _layerColliderFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "MineableLayer");
            this.fogFactory = _fogFactory;
            this.orderService = _orderService;
            this.buildingService = _buildingService;
            this.environmentService = _environmentService;
        }

        // Start is called before the first frame update
        void Start()
        {
            this.tilemap = GetComponent<Tilemap>();
            this.environmentService.GetFogObservable().Subscribe(this, fogModels =>
            {
                if (fogModels != null)
                {
                    if (this.fogObjects == null)
                    {
                        this.fogObjects = new FogObject[fogModels.GetLength(0), fogModels.GetLength(1)];
                        this.RefreshFog(fogModels);
                    }
                    else
                    {
                        this.RefreshFog(fogModels);
                    }
                }
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void RefreshFog(FogModel[,] fogModels)
        {
            if (this.tilemap != null)
            {
                IList<FogModel> objsToAdd = new List<FogModel>();
                IList<FogModel> objsToRemove = new List<FogModel>();
                for (int x = 0; x < fogModels.GetLength(0); x++)
                {
                    for (int y = 0; y < fogModels.GetLength(1); y++)
                    {
                        if (this.fogObjects[x, y] == null && fogModels[x, y] != null)
                        {
                            objsToAdd.Add(fogModels[x, y]);
                        }
                        if (this.fogObjects[x, y] != null && fogModels[x, y] == null)
                        {
                            objsToRemove.Add(fogObjects[x, y].fogModel);
                        }
                    }
                }
                objsToAdd.ForEach(fogObj =>
                {
                    this.fogObjects[fogObj.position.x, fogObj.position.y] = this.CreateFogObject(fogObj);
                });
                objsToRemove.ForEach(fogObj =>
                {
                    FogObject fogObject = this.fogObjects[fogObj.position.x, fogObj.position.y];
                    if (fogObject)
                    {
                        this.fogObjects[fogObj.position.x, fogObj.position.y] = null;
                        fogObject.Destroy();
                    }
                });
            }
        }

        private FogObject CreateFogObject(FogModel fogModel)
        {
            FogObject newFog = this.fogFactory.Create(fogModel);
            newFog.transform.position = this.tilemap.CellToLocal(fogModel.position);
            return newFog;
        }
    }
}