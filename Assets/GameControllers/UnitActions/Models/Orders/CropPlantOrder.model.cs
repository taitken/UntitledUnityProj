using System;
using UnityEngine;
using Building.Models;
using System.Collections.Generic;
using Unit.Models;
using Crops.Models;
using GameControllers.Services;

namespace GameControllers.Models
{
    public class CropPlantOrderModel : UnitOrderModel
    {
        public eCropType cropType;
        public GrowerBuildingModel growerBuilding;
        public CropPlantOrderModel(Vector3Int _coordinates, eCropType _cropType, GrowerBuildingModel _growerBuilding, bool showIcon = false) : base(_coordinates, eOrderTypes.CropPlant, showIcon)
        {
            this.cropType = _cropType;
            this.growerBuilding = _growerBuilding;
        }

        public override bool CanAssignToUnit(IList<IBaseService> _services, UnitModel _unitModel)
        {
            IItemObjectService itemSerivce = this.GetService<IItemObjectService>(_services);
            ICropService cropService = this.GetService<ICropService>(_services);
            return itemSerivce.IsItemAvailable(cropService.GetCropStats(this.cropType).seedItemType) && base.CanAssignToUnit(_services, _unitModel);
        }
    }
}

