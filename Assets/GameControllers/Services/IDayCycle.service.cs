using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Unit.Models;

namespace GameControllers.Services
{
    public interface IDayCycleService : IBaseService
    {
        public MonoObseravable<int> OnHourTickObservable { get; set; }
        public void UpdateCycle(float fixedDeltaTime);

    }
}

