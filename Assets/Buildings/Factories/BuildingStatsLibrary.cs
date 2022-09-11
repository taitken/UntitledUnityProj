
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public static class BuildingStatsLibrary
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
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 400) },
                        storageMax = 10000
                    };
                    break;
                case eBuildingType.FloorTile:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Floor Tile",
                        size = new Vector2(1, 1),
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 25) }
                    };
                    break;
                case eBuildingType.Smelter:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Smelter",
                        size = new Vector2(2, 2),
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 800) },
                        itemRecipes = new List<ItemRecipeModel>()
                            {
                                new ItemRecipeModel("Iron",
                                new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 200), new ItemObjectMass(eItemType.Coal, 25) },
                                new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Iron, 25) },
                                10 ),
                                new ItemRecipeModel("Copper",
                                new List<ItemObjectMass>() { new ItemObjectMass(eItemType.CopperOre, 200), new ItemObjectMass(eItemType.Coal, 25) },
                                new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Copper, 100) },
                                10 )
                            }

                    };
                    break;
                case eBuildingType.Torch:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Torch",
                        size = new Vector2(1, 1),
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 50) }
                    };
                    break;
                case eBuildingType.Wall:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Wall",
                        size = new Vector2(1, 1),
                        wallType = eWallTypes.Stone,
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 25) }
                    };
                    break;
                case eBuildingType.Door:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Door",
                        size = new Vector2(1, 1),
                        doorType = eDoorTypes.Stone,
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 200) }
                    };
                    break;
                case eBuildingType.FarmPlot:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Farm Plot",
                        size = new Vector2(1, 1),
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 100) }
                    };
                    break;
            }
            return buildingStats;
        }
    }
}