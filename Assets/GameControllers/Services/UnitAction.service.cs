using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;

namespace GameControllers.Services
{
    public class UnitActionService : IUnitActionService
    {
        public Observable<IList<UnitActionModel>> actionQueue { get; set; } = new Observable<IList<UnitActionModel>>(new List<UnitActionModel>());

        public void addAction(UnitActionModel action)
        {
            IList<UnitActionModel> _queue = this.actionQueue.Get();
            _queue.Add(action);
            this.actionQueue.Next(_queue);
        }
    }
}
