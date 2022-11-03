
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class BuildingStatsModel
    {
        public eBuildingType buildingType { get; set; }
        public string buildingName { get; set; }
        public Vector2 size { get; set; }
        public decimal storageMax { get; set; }
        public eBuildingCategory buildCategory { get; set; }
        public eWallTypes wallType { get; set; }
        public eDoorTypes doorType { get; set; }
        public IList<ItemObjectMass> buildSupply { get; set; }
        public IList<ItemRecipeModel> itemRecipes { get; set; }
    }
}
