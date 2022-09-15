
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
                        growTime = 24,
                        producedItems = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 400) },
                        cropType = eCropType.Blumberry,
                        seedItemType = eItemType.BlumberrySeed
                    };
                    break;
                case eCropType.Grunberry:
                    cropStats = new CropStatsModel
                    {
                        cropName = "Grunberry",
                        growTime = 48,
                        producedItems = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 400) },
                        cropType = eCropType.Grunberry,
                        seedItemType = eItemType.GrunberrySeed
                    };
                    break;
                case eCropType.Pubberbill:
                    cropStats = new CropStatsModel
                    {
                        cropName = "Pubberbill",
                        growTime = 96,
                        producedItems = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 400) },
                        cropType = eCropType.Pubberbill,
                        seedItemType = eItemType.PubberbillSeed
                    };
                    break;
                case eCropType.Luttipod:
                    cropStats = new CropStatsModel
                    {
                        cropName = "Luttipod",
                        growTime = 96,
                        producedItems = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 400) },
                        cropType = eCropType.Luttipod,
                        seedItemType = eItemType.LuttipodSeed
                    };
                    break;
            }
            return cropStats;
        }
    }
}