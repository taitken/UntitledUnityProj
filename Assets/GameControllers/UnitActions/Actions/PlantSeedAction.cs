using System;
using UnityEngine;
using GameControllers.Models;
using GameControllers.Services;
using Building.Models;
using Building;
using Unit.Models;
using System.Collections.Generic;
using Crops.Models;
using Item.Models;
using ObjectComponents;

namespace UnitAction
{
    public class PlantSeedAction : IUnitAction
    {
        private ICropService cropService;
        private CropPlantOrderModel cropPlantOrder;
        private IItemObjectService itemService;
        public UnitModel unit { get; set; }
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public PlantSeedAction(UnitModel _unit,
                                ICropService _cropService,
                                IItemObjectService _itemService)
        {
            this.unit = _unit;
            this.cropService = _cropService;
            this.itemService = _itemService;
            this.cropPlantOrder = _unit.currentOrder as CropPlantOrderModel;
        }

        public bool CheckCompleted()
        {
            return this.completed;
        }

        public void CancelAction()
        {
            this.cancel = true;
        }
        public bool PerformAction()
        {
            CropStatsModel cropStats = this.cropService.GetCropStats(this.cropPlantOrder.cropType);
            if (this.cropPlantOrder.growerBuilding.GetObjectComponent<ObjectStorageComponent>().GetItem(cropStats.seedItemType) == null)
            {
                this.cancel = true;
                Debug.LogException(new System.Exception("Plant seed action failed. Grower building does not have the correct seed."));
            }
            else
            {
                CropObjectModel newCrop = new CropObjectModel(this.cropPlantOrder.coordinates, new ItemObjectMass(eItemType.OrganicMass, 1));
                this.cropPlantOrder.growerBuilding.PlantCrop(newCrop); 
                this.cropService.AddCrop(newCrop);
                this.completed = true;
            }
            return true;
        }
    }
}