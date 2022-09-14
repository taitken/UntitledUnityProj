
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using Zenject;
using GameControllers.Models;
using UI.Services;
using Characters;
using Unit.Models;
using Building.Models;
using Environment.Models;
using System.Linq;
using System;
using Crops.Models;
using Crops;

namespace Environment
{
    public class CropLayer : MonoBehaviourLayer
    {
        private ICropService cropService;
        private IEnvironmentService envService;
        private MouseActionModel mouseAction;
        private CropObject.Factory cropFactory;
        public IList<CropObject> cropObjects = new List<CropObject>();
        private IList<CropObjectModel> cropObjectModels { get { return this.cropObjects.Map(crop => { return crop.cropObjectModel; }); } }

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IEnvironmentService _envService,
                              ICropService _cropService,
                              LayerCollider.Factory _layerColliderFactory,
                              CropObject.Factory _cropFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "CharacterLayer");
            this.envService = _envService;
            this.cropService = _cropService;
            this.cropFactory = _cropFactory;
            this.cropService.cropObseravable.Subscribe(this, this.HandleCropModels);
        }

        void Start()
        {

        }
        
        void HandleCropModels(IList<CropObjectModel> cropObjectModel)
        {
            IList<CropObjectModel> newModels = cropObjectModel.GetNewModels(this.cropObjectModels);
            IList<CropObjectModel> removedModels = cropObjectModel.GetRemovedModels(this.cropObjectModels);
            newModels.ForEach(newModel =>
            {
                this.cropObjects.Add(this.CreateCrop(newModel));
            });
            removedModels.ForEach(removedModels =>
            {
                CropObject cropObjectToRemove = this.cropObjects.Find(crop => { return crop.cropObjectModel.ID == removedModels.ID; });
                this.cropObjects.Remove(cropObjectToRemove);
                cropObjectToRemove.Destroy();
            });
        }

        private CropObject CreateCrop(CropObjectModel cropModel)
        {
            CropObject newCropObject = this.cropFactory.Create(cropModel);
            newCropObject.transform.position = this.envService.CellToLocal(cropModel.position);
            return newCropObject;
        }
    }
}