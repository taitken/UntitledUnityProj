
using System;
using GameControllers.Models;
using GameControllers.Services;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;

namespace UnitAction
{
    public class ActionSequence
    {
        private IList<Func<IUnitAction>> actionCallbacks = new List<Func<IUnitAction>>();
        private IUnitAction currentAction;
        private IUnitOrderService unitOrderService;
        private int completedActions;
        public UnitOrderModel unitOrder;
        public EventEmitter onCancel;
        public int size { get { return this.actionCallbacks.Count; } }
        public ActionSequence(IUnitOrderService _orderService, UnitOrderModel unitOrder, IUnitAction firstAction)
        {
            this.completedActions = 0;
            this.currentAction = firstAction;
            this.unitOrder = unitOrder;
            this.unitOrderService = _orderService;
            this.onCancel = new EventEmitter();
        }

        public void Update()
        {
            if (this.currentAction != null && this.currentAction.cancel)
            {
                this.onCancel.Emit();
            }
            if (this.currentAction != null && this.currentAction.CheckCompleted())
            {
                this.currentAction = this.GetNextAction();
                this.completedActions++;
                if (this.currentAction != null)
                {
                    this.TryPerformAction(this.currentAction);
                }
                else
                {
                    this.CompleteOrder();
                }
            }

        }

        public void Begin()
        {
            this.TryPerformAction(this.currentAction);
        }

        public void TryPerformAction(IUnitAction action)
        {
            action.PerformAction();
            // try
            // {
            //     action.PerformAction();
            // }
            // catch (System.Exception)
            // {
            //     action.CancelAction();
            //     throw new Exception();
            // }
        }

        private void CompleteOrder()
        {
            this.unitOrderService.RemoveOrder(this.unitOrder.ID);
        }

        public ActionSequence Then(Func<IUnitAction> nextAction)
        {
            this.actionCallbacks.Add(nextAction);
            return this;
        }

        private IUnitAction GetNextAction()
        {
            if (this.completedActions < this.actionCallbacks.Count)
            {
                return this.actionCallbacks[this.completedActions]();
            }
            return null;
        }
    }
}