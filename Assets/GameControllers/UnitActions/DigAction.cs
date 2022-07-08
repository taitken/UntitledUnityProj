
using System;
using UnityEngine;
using GameControllers.Models;
using GameControllers.Services;

namespace UnitAction
{
    public class DigAction : IUnitAction
    {
        private UnitModel unit;
        private IPathFinderService pathFinderService;
        private IEnvironmentService environmentService;
        public bool completed { get; set; } = false;
        public DigAction(UnitModel _unit,
                          IPathFinderService _pathFinderService,
                          IEnvironmentService _environmentService)
        {
            this.unit = _unit;
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
        }

        public bool CheckCompleted()
        {
            return false;
        }
        public bool PerformAction()
        {
            Debug.Log("Any diggers in the chat?");
            return true;
        }
    }
}