
using System.Collections.Generic;
using Item.Models;

namespace Item.Models
{
    public static class ItemStatsLibrary
    {
        public static IList<ItemStatsModel> GetAllItemStats()
        {
            IList<ItemStatsModel> itemStats = new List<ItemStatsModel>();
            foreach (eItemType i in eItemType.GetValues(typeof(eItemType)))
            {
                itemStats.Add(ItemStatsLibrary.GetItemStats(i));
            }
            return itemStats;
        }
        public static ItemStatsModel GetItemStats(eItemType itemType)
        {
            ItemStatsModel itemStats = null;
            switch (itemType)
            {
                case eItemType.Stone:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Stone",
                        itemDescription = "A rocky stonelike material. Probably made of stone",
                        itemCategory = eItemCategory.RawMinerals
                    };
                    break;
                case eItemType.Iron:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Iron",
                        itemDescription = "An iron ingot. Made from iron ore.",
                        itemCategory = eItemCategory.RefinedMetal
                    };
                    break;
                case eItemType.Coal:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Coal",
                        itemDescription = "Raw coal. Can be used in production of refine materials or to produce heat.",
                        itemCategory = eItemCategory.RawMinerals
                    };
                    break;
                case eItemType.CopperOre:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Copper Ore",
                        itemDescription = "Raw copper ore. Must be refined to be of some use.",
                        itemCategory = eItemCategory.RawOre
                    };
                    break;
                case eItemType.Copper:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Copper",
                        itemDescription = "Refined copper ingot. Can be used to produce items.",
                        itemCategory = eItemCategory.RefinedMetal
                    };
                    break;
                case eItemType.OrganicMass:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Organic Mass",
                        itemDescription = "Made from the goop of the earth.",
                        itemCategory = eItemCategory.Organic
                    };
                    break;
                case eItemType.BlumberrySeed:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Blumberry Seed",
                        itemDescription = "A seed used to grow delicious Blumberries.",
                        itemCategory = eItemCategory.Seeds
                    };
                    break;
                case eItemType.PubberbillSeed:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Pubberbill Seed",
                        itemDescription = "A seed used to grow scrumptious Pubberbill.",
                        itemCategory = eItemCategory.Seeds
                    };
                    break;
                case eItemType.GrunberrySeed:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "GrunberrySeed Seed",
                        itemDescription = "A seed used to grow zesty Grunberry.",
                        itemCategory = eItemCategory.Seeds
                    };
                    break;
                case eItemType.LuttipodSeed:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Luttipod Seed",
                        itemDescription = "A seed used to grow fresh Luttipod.",
                        itemCategory = eItemCategory.Seeds
                    };
                    break;
                case eItemType.Blumberry:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Blumberry",
                        itemDescription = "A delicious Blumberry.",
                        itemCategory = eItemCategory.Edible
                    };
                    break;
                case eItemType.Pubberbill:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Pubberbill",
                        itemDescription = "A scrumptious Pubberbill.",
                        itemCategory = eItemCategory.Edible
                    };
                    break;
                case eItemType.Grunberry:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Grunberry",
                        itemDescription = "A zesty Grunberry.",
                        itemCategory = eItemCategory.Edible
                    };
                    break;
                case eItemType.Luttipod:
                    itemStats = new ItemStatsModel
                    {
                        itemName = "Luttipod",
                        itemDescription = "A fresh Luttipod.",
                        itemCategory = eItemCategory.Edible
                    };
                    break;
            }
            return itemStats;
        }
    }
}