

using System;
using System.Collections.Generic;
using Item.Models;
using UtilityClasses;

namespace ObjectComponents
{
    public class ObjectNeedsComponent : ObjectComponent
    {
        private float fullnessDecay = 0.001f;
        private float currentFullness;

        public ObjectNeedsComponent() : base()
        {
            this.currentFullness = 1f;
        }

        public float GetFullness()
        {
            return this.currentFullness;
        }

        public void UpdateFullness(float multiplier)
        {
            this.currentFullness = this.currentFullness - (this.fullnessDecay * GameTime.fixedDeltaTime);
        }
    }
}
