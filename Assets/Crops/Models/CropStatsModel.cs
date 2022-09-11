using System;
using System.Collections.Generic;
using Item.Models;
using ObjectComponents;
using UnityEngine;

namespace Crops.Models
{
    public class CropStatsModel
    {
        public float growTime { get; set; }
        public IList<ItemObjectMass> producedItems { get; set; }
        public Sprite[] sprites { get; set; }
    }
}

