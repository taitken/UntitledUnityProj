using System;
using UnityEngine;
using GameControllers.Models;
using GameControllers.Services;
using Unit.Models;

namespace UnitAction
{
    public class MoveAction : IUnitAction
    {
        private UnitModel unit;
        private IPathFinderService pathFinderService;
        private IEnvironmentService environmentService;
        private Vector3Int destination;
        private bool moveOnTopOfDest { get; set; }
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public MoveAction(UnitModel _unit,
                          Vector3Int _destination,
                          IPathFinderService _pathFinderService,
                          IEnvironmentService _environmentService,
                          bool _moveOnTopOfDest)
        {
            this.unit = _unit;
            this.moveOnTopOfDest = _moveOnTopOfDest;
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
            this.destination = _destination;
        }

        public bool CheckCompleted()
        {
            if (this.unit.currentPath == null || this.unit.currentPath.Count == 0)
            {
                this.completed = true;
            }
            return this.completed;
        }
        public bool PerformAction()
        {
            unit.currentPath = this.pathFinderService.FindPath(this.environmentService.LocalToCell(unit.position),
                                                                this.destination,
                                                                this.pathFinderService.pathFinderMap.Get(),
                                                                this.moveOnTopOfDest);
            if (unit.currentPath != null)
            {
                unit.currentPath.RemoveAt(0);
            }
            return unit.currentPath != null;
        }
    }
}