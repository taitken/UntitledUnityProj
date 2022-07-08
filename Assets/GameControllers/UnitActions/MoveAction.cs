using System;
using UnityEngine;
using GameControllers.Models;
using GameControllers.Services;

namespace UnitAction
{
    public class MoveAction : IUnitAction
    {
        private UnitModel unit;
        private IPathFinderService pathFinderService;
        private IEnvironmentService environmentService;
        public bool completed { get; set; } = false;
        public MoveAction(UnitModel _unit,
                          IPathFinderService _pathFinderService,
                          IEnvironmentService _environmentService)
        {
            this.unit = _unit;
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
        }

        public bool CheckCompleted()
        {
            if(this.unit.currentPath == null || this.unit.currentPath.Count == 0)
            {
                this.completed = true;
            }
            return this.completed;
        }
        public bool PerformAction()
        {
            unit.currentPath = this.pathFinderService.FindPath(this.environmentService.LocalToCell(unit.position), unit.currentOrder.coordinates, this.pathFinderService.pathFinderMap.Get(), true);
            if (unit.currentPath != null)
            {
                unit.currentPath.RemoveAt(0);
            }
            return unit.currentPath != null;
        }
    }
}