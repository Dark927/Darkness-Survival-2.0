using UnityEngine;

namespace Utilities.Math
{
    public static class BezierUtils
    {
        public static float Bezier(float t, Vector2 firstPoint, Vector2 secondPoint)
        {
            return Bezier(t, firstPoint.x, firstPoint.y, secondPoint.x, secondPoint.y);
        }

        public static float Bezier(float t, float x1, float y1, float x2, float y2)
        {
            // Clamp t between 0 and 1
            t = Mathf.Clamp01(t);

            // Approximate x(t) using Newton's method since t represents time, not x directly
            float x = SolveBezierX(t, x1, x2);

            // Compute y using the Bézier formula
            return BezierCurve(x, y1, y2);
        }

        private static float SolveBezierX(float t, float x1, float x2, int iterations = 5)
        {
            float x = t;
            for (int i = 0; i < iterations; i++)
            {
                float fx = BezierCurve(x, x1, x2) - t;
                float dfx = BezierDerivative(x, x1, x2);
                if (Mathf.Abs(fx) < 1e-6f) break;
                x -= fx / dfx;
            }
            return x;
        }

        private static float BezierCurve(float t, float p1, float p2)
        {
            float mt = 1 - t;
            return (3 * mt * mt * t * p1) + (3 * mt * t * t * p2) + (t * t * t);
        }

        private static float BezierDerivative(float t, float p1, float p2)
        {
            float mt = 1 - t;
            return 3 * mt * mt * p1 + 6 * mt * t * (p2 - p1) + 3 * t * t * (1 - p2);
        }
    }
}