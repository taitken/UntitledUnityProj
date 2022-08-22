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
    public class DeconstructAction : IUnitAction
    {
        private IBuildingService buildingService;
        private DeconstructOrderModel deconstructOrderModel;
        public UnitModel unit { get; set; }
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public DeconstructAction(UnitModel _unit,
                                 IBuildingService _buildingService)
        {
            this.unit = _unit;
            this.buildingService = _buildingService;
            this.deconstructOrderModel = _unit.currentOrder as DeconstructOrderModel;
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
            this.buildingService.RemoveBuilding(this.deconstructOrderModel.buildingModel.ID);
            this.completed = true;
            return true;
        }
    }
}