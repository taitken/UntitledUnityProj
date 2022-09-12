using System;
using System.Collections.Generic;
using UnityEngine;
using Building.Models;
using UtilityClasses;
using Building;
using Crops.Models;
using Crops;

namespace GameControllers.Services
{
    public interface ICropService : IBaseService
    {
        MonoObseravable<IList<CropObjectModel>> cropObseravable { get; set; }
        void SetCropAssetController(CropAssetController _cropAssetController);
        Sprite[] GetCropSpriteSet(eCropType cropType);
        public IList<CropStatsModel> GetAllCropStats();
        public CropStatsModel GetCropStats(eCropType cropType);
        void AddCrop(CropObjectModel crop);
        void RemoveCrop(long id);
    }
}
