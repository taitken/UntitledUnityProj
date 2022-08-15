using Building.Models;
using UnityEngine;

namespace Building
{
    public class WallBuildingObject : BuildingObject
    {
        public WallBuildingModel wallBuildingModel;
        public Sprite[] spriteList;
        protected override void OnCreation()
        {
            this.wallBuildingModel = this.buildingObjectModel as WallBuildingModel;
            this.spriteList = this.buildingService.GetWallSprites(this.wallBuildingModel.wallType);
        }

        public void UpdateSprite(int spriteID)
        {
            this.GetComponent<SpriteRenderer>().sprite = this.spriteList[spriteID];
        }
    }
}