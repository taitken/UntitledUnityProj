using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;

namespace GameControllers.Services
{
    public class UnitActionService : IUnitActionService
    {
        public Subscribable<eMouseAction> mouseAction {get;set;} =  new Subscribable<eMouseAction>(eMouseAction.None);

        public Subscribable<IList<UnitActionModel>> actionQueue { get; set; } = new Subscribable<IList<UnitActionModel>>(new List<UnitActionModel>());

        public void addAction(UnitActionModel action)
        {
            IList<UnitActionModel> _queue = this.actionQueue.Get();
            _queue.Add(action);
            this.actionQueue.Set(_queue);
        }
    }
}
