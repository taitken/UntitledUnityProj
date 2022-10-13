
using System;
using UnityEngine;
using System.Collections.Generic;
using Environment.Models;
using GameControllers.Models;
using GameControllers.Services;
using Unit.Models;

namespace UnitAction
{
    public class DigAction : IUnitAction
    {
        private IPathFinderService pathFinderService;
        private IEnvironmentService environmentService;
        private DigOrderModel digOrder;
        public UnitModel unit { get; set; }
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public DigAction(UnitModel _unit,
                          IPathFinderService _pathFinderService,
                          IEnvironmentService _environmentService)
        {
            this.unit = _unit;
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
            this.digOrder = this.unit.currentOrder as DigOrderModel;
        }
        public bool CheckCompleted()
        {
            return this.environmentService.mineableObjects.Get()[this.unit.currentOrder.coordinates.x, this.unit.currentOrder.coordinates.y] == null;
        }
        public void CancelAction()
        {
            this.cancel = true;
        }
        public bool PerformAction()
        {
            this.unit.unitState = eUnitState.Digging;
            //if (mineableObj != null) this.environmentService.RemoveMineableObject(mineableObj.position);
            return true;
        }
    }
}