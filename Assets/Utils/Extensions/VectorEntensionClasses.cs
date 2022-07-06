using UnityEngine;

namespace System.Collections.Generic
{
    public static class VectorExtenionClasses
    {
        public static Vector2 GetDirection(this Vector3 origin, Vector3 destination)
        {
            float x = 0;
            float y = 0;
            if (origin.x < destination.x)
            {
                x = 1;
            }
            if (origin.x > destination.x)
            {
                x = -1;
            }
            if (origin.y < destination.y)
            {
                y = 1;
            }
            if (origin.y > destination.y)
            {
                y = -1;
            }
            if (Math.Abs(origin.x - destination.x) < 0.02f)
            {
                x = 0;
            }
            if (Math.Abs(origin.y - destination.y) < 0.02f)
            {
                y = 0;
            }
            return new Vector2(x, y);
        }
    }
}