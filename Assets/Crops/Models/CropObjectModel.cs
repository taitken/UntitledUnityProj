using System;
using System.Collections.Generic;
using Item.Models;
using ObjectComponents;
using UnityEngine;

namespace Crops.Models
{
    public class CropObjectModel : BaseObjectModel
    {
        public CropObjectModel(Vector3Int _position, ItemObjectMass _seed) : base(_position, new List<ItemObjectMass> { _seed })
        {

        }
        public float growTime { get; set; }
        private CropStatsModel cropStats { get; set; }
        public IList<ItemObjectMass> producedItems { get; set; }
        public Sprite[] sprites { get; set; }
    }
}

