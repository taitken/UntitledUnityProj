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
        Func<bool> completeCondition { get; set; }

        public ActionFactory(IPathFinderService _pathFinderService,
                             IEnvironmentService _environmentService)
        {
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
        }

        public ActionSequence CreateSequence(UnitModel _unit)
        {
            ActionSequence newSequence = null;
            switch (_unit.currentOrder.orderType)
            {
                case eOrderTypes.Dig:
                    newSequence = new ActionSequence(_unit.currentOrder, new MoveAction(_unit, this.pathFinderService, this.environmentService))
                                    .Then(new DigAction(_unit, this.pathFinderService, this.environmentService));
                    break;
            }
            return newSequence;
        }
    }
}