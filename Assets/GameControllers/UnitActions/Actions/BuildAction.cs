using System;
using UnityEngine;
using GameControllers.Models;
using GameControllers.Services;
using Building.Models;
using Building;
using Unit.Models;
using System.Collections.Generic;

namespace UnitAction
{
    public class BuildAction : IUnitAction
    {
        private UnitModel unit;
        private IBuildingService buildingService;
        private BuildOrderModel buildOrder;
        private BuildingObjectFactory buildingFactory;
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public BuildAction(UnitModel _unit,
                          IBuildingService _buildingService)
        {
            this.unit = _unit;
            this.buildingService = _buildingService;
            this.buildOrder = _unit.currentOrder as BuildOrderModel;
            this.buildingFactory = new BuildingObjectFactory();
        }

        public bool CheckCompleted()
        {
            return this.completed;
        }

        public void CancelAction()
        {
            this.cancel = true;
        }
        public bool PerformAction()
        {
            this.buildingService.RemoveBuildSite(this.buildingService.buildingSiteObseravable.Get().Find(site => { return site.position == this.buildOrder.coordinates; }).ID);
            this.buildingService.AddBuilding(this.buildingFactory.CreateBuildingModel(this.buildOrder.coordinates, this.buildOrder.buildingType));
            this.completed = true;
            return true;
        }
    }
}