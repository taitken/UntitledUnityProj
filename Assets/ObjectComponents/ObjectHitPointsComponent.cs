

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

        public ObjectHitPointsComponent(float startingHitPoints) : base()
        {
            this._maxHitPoints = startingHitPoints;
            this._currentHitPoints = startingHitPoints;
        }

        public void TakeDamage(float damage)
        {
            this._currentHitPoints = Math.Max(0, this._currentHitPoints - damage);
        }

    }
}
