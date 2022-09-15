using System;
using System.Collections.Generic;
using Item.Models;
using ObjectComponents;
using UnityEngine;

namespace Crops.Models
{
    public class CropObjectModel : BaseObjectModel
    {
        public const int NUM_GROW_STAGES = 7;
        public const int COMPLETED_GROW_STAGE = 5;
        public CropObjectModel(Vector3Int _position, ItemObjectMass _seed, CropStatsModel cropStats) : base(_position, new List<ItemObjectMass> { _seed })
        {
            this.growStage = 1;
            this.cropType = cropStats.cropType;
            this.growTime = cropStats.growTime;
            this.producedItems = cropStats.producedItems;
        }
        public eCropType cropType { get; set; }
        public int growTime { get; set; } // Time in hour ticks
        public int growTicks { get; set; }
        public float growStage { get; set; }
        public IList<ItemObjectMass> producedItems { get; set; }
    }
}

