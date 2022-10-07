using System;
using UnityEngine;
using GameControllers.Models;
using GameControllers.Services;
using Unit.Models;
using System.Collections.Generic;

namespace UnitAction
{
    public class MoveAction : IUnitAction
    {
        private IPathFinderService pathFinderService;
        private IEnvironmentService environmentService;
        private Vector3Int destination;
        public UnitModel unit { get; set; }
        private bool MoveAdjacentToDest { get; set; }
        private bool actionStarted { get; set; } = false;
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public MoveAction(UnitModel _unit,
                          Vector3Int _destination,
                          IPathFinderService _pathFinderService,
                          IEnvironmentService _environmentService,
                          bool _MoveAdjacentToDest)
        {
            this.unit = _unit;
            this.MoveAdjacentToDest = _MoveAdjacentToDest;
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
            this.destination = _destination;
        }

        public bool CheckCompleted()
        {
            if (this.actionStarted && (this.unit.currentPath == null || this.unit.currentPath.Count == 0))
            {
                if ((this.MoveAdjacentToDest == false && this.unit.position == this.destination) ||
                    (this.MoveAdjacentToDest && (
                        this.unit.position == this.destination - new Vector3Int(1, 0) ||
                        this.unit.position == this.destination + new Vector3Int(1, 0) ||
                        this.unit.position == this.destination - new Vector3Int(0, 1) ||
                        this.unit.position == this.destination + new Vector3Int(0, 1) ||
                        this.unit.position == this.destination
                    ))
                )
                {
                    this.completed = true;
                }
                else
                {
                    this.CancelAction();
                }
            }
            return this.completed;
        }

        public void CancelAction()
        {
            this.cancel = true;
        }

        public bool PerformAction()
        {
            unit.currentPath = this.pathFinderService.FindPath(unit.position,
                                                                this.destination,
                                                                this.pathFinderService.GetPathFinderMap(),
                                                                this.MoveAdjacentToDest);
            if (unit.currentPath != null)
            {
                unit.currentPath.RemoveAt(0);
            }
            else if (unit.position != this.destination)
            {
                this.CancelAction();
            }
            this.actionStarted = true;
            return unit.currentPath != null;
        }
    }
}