
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
                case eCropType.Blumberry:
                    cropStats = new CropStatsModel
                    {
                        cropName = "Blumberry",
                        growTime = 2f,
                        producedItems = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 400) },
                        sprites = new Sprite[7],
                        cropType = eCropType.Blumberry
                    };
                    break;
                case eCropType.Grunberry:
                    cropStats = new CropStatsModel
                    {
                        cropName = "Grunberry",
                        growTime = 4f,
                        producedItems = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 400) },
                        sprites = new Sprite[7],
                        cropType = eCropType.Grunberry
                    };
                    break;
                case eCropType.Pubberbill:
                    cropStats = new CropStatsModel
                    {
                        cropName = "Pubberbill",
                        growTime = 4f,
                        producedItems = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 400) },
                        sprites = new Sprite[7],
                        cropType = eCropType.Pubberbill
                    };
                    break;
                case eCropType.Luttipod:
                    cropStats = new CropStatsModel
                    {
                        cropName = "Luttipod",
                        growTime = 4f,
                        producedItems = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 400) },
                        sprites = new Sprite[7],
                        cropType = eCropType.Luttipod
                    };
                    break;
            }
            return cropStats;
        }
    }
}