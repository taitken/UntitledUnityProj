

using System;
using System.Collections.Generic;
using Item.Models;
using UtilityClasses;

namespace ObjectComponents
{
    public class ObjectHitPointsComponent : ObjectComponent
    {
        public float currentHitPoints { get { return this._currentHitPoints; } }
        public float maxHitPoints { get { return this._maxHitPoints; } }
        private float _currentHitPoints { get; set; }
        private float _maxHitPoints { get; set; }
        private EventEmitter onZeroHitpointsEmitter { get; set; }

        public ObjectHitPointsComponent() : base()
        {
            this._maxHitPoints = 100;
            this._currentHitPoints = 100;
            this.onZeroHitpointsEmitter = new EventEmitter();
        }

        public void Initalise(float startingHitPoints)
        {
            this._maxHitPoints = startingHitPoints;
            this._currentHitPoints = startingHitPoints;
        }

        public void TakeDamage(float damage)
        {
            this._currentHitPoints = Math.Max(0, this._currentHitPoints - damage);
            if(this._currentHitPoints <= 0)
            {
                this.onZeroHitpointsEmitter.Emit();
            }
        }

        public void OnZeroHitPoints(Action action)
        {
            this.onZeroHitpointsEmitter.OnEmit(action);
        }

    }
}
