using System.Collections.Generic;
using GameControllers.Models;
using Unit.Models;
using UnityEngine;
using UtilityClasses;

namespace Characters
{
    public class UnitStateIdle : BaseUnitState
    {
        private float idleTime;
        public UnitStateIdle() : base(eUnitState.Idle)
        {
            this.idleTime = 0;
        }

        public override void Initialise(WorldCharacter worldChar)
        {

        }
        public override void Update(WorldCharacter worldChar)
        {

        }

        public override void FixedUpdate(WorldCharacter worldChar)
        {

        }


    }
}