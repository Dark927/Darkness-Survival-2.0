using UnityEngine;

namespace Utilities.World
{
    public class PositionGenerator
    {
        public static float CalculateSafeRadius(Camera camera, float padding)
        {
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;
            float distanceToCorner = Mathf.Sqrt((halfWidth * halfWidth) + (halfHeight * halfHeight));
            return distanceToCorner + padding;
        }

        // Generates a random point inside a specific degree slice of the spawn ring
        public static Vector2 GetRandomPositionInRingArc(Vector2 center, float minRadius, float maxRadius, float minAngleDeg, float maxAngleDeg)
        {
            float randomAngle = Random.Range(minAngleDeg, maxAngleDeg) * Mathf.Deg2Rad;
            float randomRadius = Mathf.Sqrt(Random.Range(minRadius * minRadius, maxRadius * maxRadius));

            float x = center.x + (Mathf.Cos(randomAngle) * randomRadius);
            float y = center.y + (Mathf.Sin(randomAngle) * randomRadius);

            return new Vector2(x, y);
        }


        // -------------------------
        // 70/30 Directional Logic
        // -------------------------
        public static Vector2 GetDirectionalSpawnPosition(Camera camera, float safePadding, float ringThickness)
        {
            float safeRadius = CalculateSafeRadius(camera, safePadding);
            float maxRadius = safeRadius + ringThickness;
            Vector2 center = camera.transform.position;


            return GetRandomPositionInRingArc(center, safeRadius, maxRadius, 0f, 360f);
        }

        public static Vector2 GetDirectionalSpawnPosition(Camera camera, float safePadding, float ringThickness, Vector2 moveDirection, float frontalChance, float coneAngle)
        {
            float safeRadius = CalculateSafeRadius(camera, safePadding);
            float maxRadius = safeRadius + ringThickness;
            Vector2 center = camera.transform.position;

            // Find the exact angle the player is moving towards
            float baseAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            float halfCone = coneAngle / 2f;

            // Roll the dice for 70/30 distribution
            if (Random.value <= frontalChance)
            {
                // Spawn inside the Frontal Cone (e.g., -45 to +45 degrees from movement vector)
                return GetRandomPositionInRingArc(center, safeRadius, maxRadius, baseAngle - halfCone, baseAngle + halfCone);
            }
            else
            {
                // Spawn in the Flanks/Rear (The rest of the circle outside the cone)
                return GetRandomPositionInRingArc(center, safeRadius, maxRadius, baseAngle + halfCone, baseAngle + 360f - halfCone);
            }
        }
    }
}
