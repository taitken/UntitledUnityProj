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
        }

    }
}