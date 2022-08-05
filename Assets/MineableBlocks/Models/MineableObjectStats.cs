using Item.Models;

namespace MineableBlocks.Models
{
    public class MineableBlockStatsModel
    {
        public string name { get; set; }
        public eItemType dropType { get; set; }
        public int minMass { get; set; }
        public int maxMass { get; set; }
    }

    public static class MineableBlockTypeStats
    {
        public static MineableBlockStatsModel GetMineableBlockStats(eMineableBlockType mineableObjectType)
        {
            MineableBlockStatsModel mineableBlockStats = null;
            switch (mineableObjectType)
            {
                case eMineableBlockType.Stone:
                    mineableBlockStats = new MineableBlockStatsModel
                    {
                        name = "Stone Block",
                        dropType = eItemType.Stone,
                        minMass = 200,
                        maxMass = 400
                    };
                    break;
                case eMineableBlockType.Coal:
                    mineableBlockStats = new MineableBlockStatsModel
                    {
                        name = "Coal Block",
                        dropType = eItemType.Coal,
                        minMass = 400,
                        maxMass = 800
                    };
                    break;
                case eMineableBlockType.Copper:
                    mineableBlockStats = new MineableBlockStatsModel
                    {
                        name = "Copper Block",
                        dropType = eItemType.CopperOre,
                        minMass = 800,
                        maxMass = 1200
                    };
                    break;
            }
            return mineableBlockStats;
        }
    }
}