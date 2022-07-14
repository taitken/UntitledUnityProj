using System;
using UnityEngine;
using GameControllers.Models;
using GameControllers.Services;
using Building.Models;

namespace UnitAction
{
    public class BuildAction : IUnitAction
    {
        private UnitModel unit;
        private IPathFinderService pathFinderService;
        private IBuildingService buildingService;
        private BuildOrderModel buildOrder;
        public bool completed { get; set; } = false;
        public BuildAction(UnitModel _unit,
                          IBuildingService _buildingService)
        {
            this.unit = _unit;
            this.buildingService = _buildingService;
            this.buildOrder = _unit.currentOrder as BuildOrderModel;
        }

        public bool CheckCompleted()
        {
            return this.completed;
        }
        public bool PerformAction()
        {
            this.buildingService.AddBuilding(new BuildingObjectModel(this.buildOrder.coordinates, this.buildOrder.buildingType));
            this.completed = true;
            return true;
        }
    }
}