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
    public class RemoveSeedAction : IUnitAction
    {
        private ICropService cropService;
        private CropRemoveOrderModel cropPlantOrder;
        private IItemObjectService itemService;
        public UnitModel unit { get; set; }
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public RemoveSeedAction(UnitModel _unit,
                                ICropService _cropService,
                                IItemObjectService _itemService)
        {
            this.unit = _unit;
            this.cropService = _cropService;
            this.itemService = _itemService;
            this.cropPlantOrder = _unit.currentOrder as CropRemoveOrderModel;
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
            if (this.cropPlantOrder.growerBuilding.cropObject == null)
            {
                this.cancel = true;
                Debug.LogException(new System.Exception("Remove seed action failed. Grower building does not have a seed."));
            }
            else
            {
                IList<ItemObjectModel> storedSeeds = new List<ItemObjectModel>();
                this.cropPlantOrder.growerBuilding.GetObjectComponent<ObjectStorageComponent>().GetItems().ForEach(item => { storedSeeds.Add(item); });
                storedSeeds.ForEach(seed =>
                {
                    this.cropPlantOrder.growerBuilding.GetObjectComponent<ObjectStorageComponent>().RemoveItem(seed);
                    seed.itemState = ItemObjectModel.eItemState.OnGround;
                    this.itemService.AddItemToWorld(seed);
                });
                this.cropService.RemoveCrop(this.cropPlantOrder.growerBuilding.cropObject.ID);
                this.cropPlantOrder.growerBuilding.cropObject = null;
                this.cropPlantOrder.growerBuilding.NotifyModelUpdate();
                this.completed = true;
            }
            return true;
        }
    }
}