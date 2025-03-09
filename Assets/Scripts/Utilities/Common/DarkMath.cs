using UnityEngine;

namespace Utilities
{
    public static class DarkMath
    {
        public static float Frac(float value)
        {
            return value - Mathf.Floor(value);
        }
    }
}
