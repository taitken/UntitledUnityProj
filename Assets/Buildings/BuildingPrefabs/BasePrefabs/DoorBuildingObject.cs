using System.Collections.Generic;
using Building.Models;
using UnityEngine;

namespace Building
{
    public class DoorBuildingObject : BuildingObject
    {
        public DoorBuildingModel doorBuildingModel;
        protected override void OnCreation()
        {
            this.doorBuildingModel = this.buildingObjectModel as DoorBuildingModel;
            this.RotateDoor();
        }

        private void RotateDoor()
        {
            IList<WallBuildingModel> wallBuilding = this.buildingService.GetBuildings<WallBuildingModel>();
            if (wallBuilding.Find(wall =>
                {
                    return wall.position == new Vector3Int(this.doorBuildingModel.position.x, this.doorBuildingModel.position.y + 1)
                        || wall.position == new Vector3Int(this.doorBuildingModel.position.x, this.doorBuildingModel.position.y - 1);
                }) != null)
            {
                this.transform.Rotate(new Vector3(0, 0, 90));
            }
        }
    }
}