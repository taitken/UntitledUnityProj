using System;
using System.Collections.Generic;
using Characters.Utils;
using GameControllers.Services;
using UnityEngine;

namespace UtilityClasses
{
    public static class MovementSingleton
    {
        private static MovementHelper moveHelper { get; set; }

        public static void SetMovementHelper(MovementHelper _moveHelper)
        {
            MovementSingleton.moveHelper = _moveHelper;
        }

        public static MovementHelper GetMovementHelper()
        {
            return moveHelper;
        }
    }

}