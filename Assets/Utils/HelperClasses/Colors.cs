using System;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityClasses
{
    public class GameColors
    {
        public static Color AddTransparency(Color _oldColor, float _transparency)
        {
            return new Color(_oldColor.r, _oldColor.g, _oldColor.b, _transparency);
        }
    }
}