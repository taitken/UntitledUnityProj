
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public enum eBuildingType
    {
        none,
        Chest,
        FloorTile,
        Smelter
    }

    public class BuildingStatsModel
    {
        public string buildingName { get; set; }
        public Vector2 size { get; set; }
        public decimal storageMax { get; set; }
        public IList<BuildingSupply> buildSupply { get; set; }
        public IList<ItemRecipeModel> itemRecipes { get; set; }
    }

    public static class BuildingTypeStats
    {
        public static BuildingStatsModel GetBuildingStats(eBuildingType buildingType)
        {
            BuildingStatsModel buildingStats = null;
            switch (buildingType)
            {
                case eBuildingType.Chest:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Chest",
                        size = new Vector2(1, 1),
                        buildSupply = new List<BuildingSupply>() { new BuildingSupply(eItemType.Stone, 400) },
                        storageMax = 10000
                    };
                    break;
                case eBuildingType.FloorTile:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Floor Tile",
                        size = new Vector2(1, 1),
                        buildSupply = new List<BuildingSupply>() { new BuildingSupply(eItemType.Stone, 50) }
                    };
                    break;
                case eBuildingType.Smelter:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Smelter",
                        size = new Vector2(2, 2),
                        buildSupply = new List<BuildingSupply>() { new BuildingSupply(eItemType.Stone, 800) },
                        itemRecipes = new List<ItemRecipeModel>()
                            {
                                new ItemRecipeModel("Iron",
                                new List<BuildingSupply>() { new BuildingSupply(eItemType.Stone, 200), new BuildingSupply(eItemType.Coal, 25) },
                                new List<BuildingSupply>() { new BuildingSupply(eItemType.Iron, 25) },
                                10 ),
                                new ItemRecipeModel("Copper",
                                new List<BuildingSupply>() { new BuildingSupply(eItemType.CopperOre, 200), new BuildingSupply(eItemType.Coal, 25) },
                                new List<BuildingSupply>() { new BuildingSupply(eItemType.Copper, 100) },
                                10 )
                            }

                    };
                    break;
            }
            return buildingStats;
        }
    }
}
