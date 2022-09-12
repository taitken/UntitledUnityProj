using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using UnityEngine.Tilemaps;
using Crops.Models;
using Crops;

namespace GameControllers.Services
{
    public class CropService : BaseService, ICropService
    {
        private CropAssetController cropAssetController { get; set; }
        public MonoObseravable<IList<CropObjectModel>> cropObseravable { get; set; } = new MonoObseravable<IList<CropObjectModel>>(new List<CropObjectModel>());

        public void SetCropAssetController(CropAssetController _cropAssetController)
        {
            this.cropAssetController = _cropAssetController;
            this.cropAssetController.Initialise();
        }
        public Sprite[] GetCropSpriteSet(eCropType cropType)
        {
            return this.cropAssetController.GetCropSpriteSet(cropType);
        }
        public IList<CropStatsModel> GetAllCropStats()
        {
            return CropStatsLibrary.GetAllCropStats();
        }
        public CropStatsModel GetCropStats(eCropType cropType)
        {
            return CropStatsLibrary.GetCropStats(cropType);
        }
        public void AddCrop(CropObjectModel crop)
        {
            IList<CropObjectModel> _crops = this.cropObseravable.Get();
            if (crop != null && _crops.Find(existingUnit => { return crop.ID == existingUnit.ID; }) == null)
            {
                _crops.Add(crop);
                this.cropObseravable.Set(_crops);
            }
        }
        public void RemoveCrop(long id)
        {
            CropObjectModel removedCrop = this.cropObseravable.Get().Find(Crop => { return Crop.ID == id; });
            this.cropObseravable.Set(this.cropObseravable.Get().Filter(Crop => { return Crop.ID != id; }));
        }

    }
}