using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Unit.Models;

namespace GameControllers.Services
{
    public class DayCycleService : BaseService, IDayCycleService
    {
        private const float HOUR_INTERVAL = 5;
        private int currentHour = 0;
        private float timeCounter = 0;

        public DayCycleService()
        {

        }
        public MonoObseravable<int> OnHourTickObservable { get; set; } = new MonoObseravable<int>(0);

        public void UpdateCycle(float fixedDeltaTime)
        {
            this.timeCounter = this.timeCounter + fixedDeltaTime;
            if (this.timeCounter > HOUR_INTERVAL)
            {
                this.timeCounter = this.timeCounter - HOUR_INTERVAL;
                this.currentHour = this.currentHour + 1;
                if (this.currentHour == 24) this.currentHour = this.currentHour - 24;
                this.OnHourTickObservable.Set(this.currentHour);
            }
        }


    }
}
