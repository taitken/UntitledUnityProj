
using System;
using UnityEngine;
using System.Collections.Generic;
using Environment.Models;
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
            return this.environmentService.mineableObjects.Get().Find(obj =>{return obj.position == this.unit.currentOrder.coordinates;}) == null;
        }
        public bool PerformAction()
        {
            MineableObjectModel mineableObj = this.environmentService.mineableObjects.Get().Find(obj =>{return obj.position == this.unit.currentOrder.coordinates;});
            this.environmentService.RemoveMineableObject(mineableObj.ID);
            return true;
        }
    }
}