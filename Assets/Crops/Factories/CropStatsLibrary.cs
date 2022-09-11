
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Crops.Models
{
    public static class CropStatsLibrary
    {
        public static IList<CropStatsModel> GetAllCropStats()
        {
            IList<CropStatsModel> cropStats = new List<CropStatsModel>();
            foreach (eCropType i in eCropType.GetValues(typeof(eCropType)))
            {
                cropStats.Add(CropStatsLibrary.GetCropStats(i));
            }
            return cropStats;
        }
        public static CropStatsModel GetCropStats(eCropType cropType)
        {
            CropStatsModel cropStats = null;
            switch (cropType)
            {
                case eCropType.Acorn:
                    cropStats = new CropStatsModel
                    {
                        growTime = 2f,
                        producedItems = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 400) },
                        sprites = new Sprite[7]
                    };
                    break;
                case eCropType.Flower:
                    cropStats = new CropStatsModel
                    {
                        growTime = 4f,
                        producedItems = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 400) },
                        sprites = new Sprite[7]
                    };
                    break;
            }
            return cropStats;
        }
    }
}