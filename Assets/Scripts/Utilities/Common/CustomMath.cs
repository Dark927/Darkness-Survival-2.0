﻿
using UnityEngine;

namespace Utilities.Math
{
    public class CustomMath
    {
        public static float InverseLerpUnclamped(float a, float b, float value)
        {
            if (a == b) return 0; //prevent division by zero

            return (value - a) / (b - a);
        }
        public static float Frac(float value)
        {
            return value - Mathf.Floor(value);
        }
    }
}
