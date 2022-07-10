using System;
using GameControllers.Models;
using GameControllers.Services;

namespace UnitAction
{
    public enum eActionTypes
    {
        moveAction,
        digAction
    }
    public class ActionFactory
    {
        IPathFinderService pathFinderService;
        IEnvironmentService environmentService;
        IUnitOrderService orderService;
        Func<bool> completeCondition { get; set; }

        public ActionFactory(IPathFinderService _pathFinderService,
                             IEnvironmentService _environmentService,
                             IUnitOrderService _orderService)
        {
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
            this.orderService = _orderService;
        }

        public ActionSequence CreateSequence(UnitModel _unit)
        {
            ActionSequence newSequence = null;
            switch (_unit.currentOrder.orderType)
            {
                case eOrderTypes.Dig:
                    newSequence = new ActionSequence(this.orderService,_unit.currentOrder, new MoveAction(_unit, this.pathFinderService, this.environmentService))
                        .Then(new DigAction(_unit, this.pathFinderService, this.environmentService));
                    break;
            }
            return newSequence;
        }
    }
}