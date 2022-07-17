using System;
using UnityEngine;
using GameControllers.Models;
using GameControllers.Services;
using Building.Models;
using Building;
using Unit.Models;

namespace UnitAction
{
    public class BuildAction : IUnitAction
    {
        private UnitModel unit;
        private IPathFinderService pathFinderService;
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
        public bool PerformAction()
        {
            this.buildingService.AddBuilding(this.buildingFactory.CreateBuildingModel(this.buildOrder.coordinates, this.buildOrder.buildingType));
            this.completed = true;
            return true;
        }
    }
}