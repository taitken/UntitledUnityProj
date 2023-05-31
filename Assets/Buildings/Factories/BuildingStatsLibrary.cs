
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public static class BuildingStatsLibrary
    {
        public static IList<BuildingStatsModel> GetBuildingStats()
        {
            IList<BuildingStatsModel> buildStats = new List<BuildingStatsModel>();
            foreach (eBuildingType i in eBuildingType.GetValues(typeof(eBuildingType)))
            {
                buildStats.Add(BuildingStatsLibrary.GetBuildingStats(i));
            }
            return buildStats;
        }
        public static IList<BuildingStatsModel> GetBuildingStats(eBuildingCategory buildingCategory)
        {
            return BuildingStatsLibrary.GetBuildingStats().Filter(buildStats => { return buildStats?.buildCategory == buildingCategory; });
        }
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
                        storageMax = 10000,
                        buildCategory = eBuildingCategory.Storage
                    };
                    break;
                case eBuildingType.BarracksFloorTile:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Barracks Tile",
                        size = new Vector2(1, 1),
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 25) },
                        buildCategory = eBuildingCategory.FloorTile,
                        floorType = eFloorType.Barracks
                    };
                    break;
                case eBuildingType.DiningFloorTile:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Dining Tile",
                        size = new Vector2(1, 1),
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 25) },
                        buildCategory = eBuildingCategory.FloorTile,
                        floorType = eFloorType.Dining
                    };
                    break;
                case eBuildingType.FoundryFloorTile:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Foundry Tile",
                        size = new Vector2(1, 1),
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 25) },
                        buildCategory = eBuildingCategory.FloorTile,
                        floorType = eFloorType.Foundry
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
                            },
                        buildCategory = eBuildingCategory.Production

                    };
                    break;
                case eBuildingType.Torch:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Torch",
                        size = new Vector2(1, 1),
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 50) },
                        buildCategory = eBuildingCategory.Decor
                    };
                    break;
                case eBuildingType.Wall:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Wall",
                        size = new Vector2(1, 1),
                        wallType = eWallTypes.Stone,
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 25) },
                        buildCategory = eBuildingCategory.Wall
                    };
                    break;
                case eBuildingType.Door:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Door",
                        size = new Vector2(1, 1),
                        doorType = eDoorTypes.Stone,
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 200) },
                        buildCategory = eBuildingCategory.Door
                    };
                    break;
                case eBuildingType.FarmPlot:
                    buildingStats = new BuildingStatsModel
                    {
                        buildingName = "Farm Plot",
                        size = new Vector2(1, 1),
                        buildSupply = new List<ItemObjectMass>() { new ItemObjectMass(eItemType.Stone, 100) },
                        buildCategory = eBuildingCategory.Grower
                    };
                    break;
            }
            if (buildingStats != null) buildingStats.buildingType = buildingType;
            return buildingStats;
        }
    }
}