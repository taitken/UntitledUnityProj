using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;

namespace GameControllers.Services
{
    public class UnitActionService : IUnitActionService
    {
        public Subscribable<IList<UnitActionModel>> actionQueue { get; set; } = new Subscribable<IList<UnitActionModel>>(new List<UnitActionModel>());

        public void addAction(UnitActionModel action)
        {
            IList<UnitActionModel> _queue = this.actionQueue.Get();
            _queue.Add(action);
            this.actionQueue.Set(_queue);
        }
    }
}
