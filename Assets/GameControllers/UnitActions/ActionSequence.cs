
using System;
using GameControllers.Models;
using GameControllers.Services;
using System.Collections.Generic;
using UnityEngine;

namespace UnitAction
{
    public class ActionSequence
    {
        private IList<IUnitAction> actions = new List<IUnitAction>();
        private IUnitAction currentAction;
        public UnitOrderModel unitOrder;
        public bool completed {
            get
            {
                return this.actions.Find(action =>{return action.CheckCompleted() == false;}) == null;
            }
        }
        public ActionSequence(UnitOrderModel unitOrder, IUnitAction firstAction)
        {
            this.actions.Add(firstAction);
            this.currentAction = firstAction;
            this.unitOrder = unitOrder;
        }

        public void Update()
        {
            if(this.currentAction != null && this.currentAction.CheckCompleted())
            {
                this.currentAction = this.GetNextAction();
                this.TryPerformAction(this.currentAction);
            }
        }

        public void Begin()
        {
            this.TryPerformAction(this.actions[0]);
        }

        public void TryPerformAction(IUnitAction action)
        {
            if(action.PerformAction() == false)
            {
                
            }
        }

        public ActionSequence Then(IUnitAction nextAction)
        {
            this.actions.Add(nextAction);
            return this;
        }

        private IUnitAction GetNextAction()
        {
            return this.actions.Find(action =>{return action.completed == false;});
        }
    }
}