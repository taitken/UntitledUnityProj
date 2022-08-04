using Item.Models;

namespace MineableBlocks.Models
{
    public class MineableBlockStatsModel
    {
        public string name { get; set; }
        public eItemType dropType { get; set; }
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
                        dropType = eItemType.Stone
                    };
                    break;
                case eMineableBlockType.Coal:
                    mineableBlockStats = new MineableBlockStatsModel
                    {
                        name = "Coal Block",
                        dropType = eItemType.Coal
                    };
                    break;
            }
            return mineableBlockStats;
        }
    }
}