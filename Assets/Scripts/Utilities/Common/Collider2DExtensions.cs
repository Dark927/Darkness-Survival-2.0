using UnityEngine;

namespace Utilities
{
    public static class Collider2DExtensions
    {
        /// <summary>
        /// Copy parameters from source Collider2D to target
        /// </summary>
        public static void CopyPropertiesFrom(this Collider2D target, Collider2D source)
        {
            if (source == null || target == null)
            {
                Debug.LogWarning("# source or target collider is null, can not copy!");
                return;
            }

            target.isTrigger = source.isTrigger;
            target.usedByEffector = source.usedByEffector;
            target.offset = source.offset;


            // Copy unique parameters for different types of Collider 2D

            if (source is BoxCollider2D srcBox && target is BoxCollider2D tgtBox)
            {
                tgtBox.size = srcBox.size;
                target.usedByComposite = source.usedByComposite;
            }
            else if (source is CircleCollider2D srcCircle && target is CircleCollider2D tgtCircle)
            {
                tgtCircle.radius = srcCircle.radius;
            }
            else if (source is CapsuleCollider2D srcCapsule && target is CapsuleCollider2D tgtCapsule)
            {
                tgtCapsule.size = srcCapsule.size;
                tgtCapsule.direction = srcCapsule.direction;
            }
            else if (source is PolygonCollider2D srcPolygon && target is PolygonCollider2D tgtPolygon)
            {
                tgtPolygon.points = (Vector2[])srcPolygon.points.Clone();
            }
            else
            {
                Debug.LogWarning($" # Type of collider -> {source.GetType()} is not supported for requested copy!");
            }
        }


        /// <summary>
        /// Scale Collider2D using the scale parameter
        /// </summary>
        public static void ScaleCollider2D(this Collider2D collider, float scale)
        {
            if (collider == null)
            {
                Debug.LogWarning("# Collider is null, can not scale it!");
                return;
            }

            if (scale <= 0)
            {
                Debug.LogWarning(" # Scale must be greater than zero!");
                return;
            }

            // Apply the scale for different types of Collider 2D

            if (collider is BoxCollider2D boxCollider)
            {
                boxCollider.size *= scale;
            }
            else if (collider is CircleCollider2D circleCollider)
            {
                circleCollider.radius *= scale;
            }
            else if (collider is CapsuleCollider2D capsuleCollider)
            {
                capsuleCollider.size *= scale;
            }
            else if (collider is PolygonCollider2D polygonCollider)
            {
                Vector2[] newPoints = new Vector2[polygonCollider.points.Length];
                for (int i = 0; i < polygonCollider.points.Length; i++)
                {
                    newPoints[i] = polygonCollider.points[i] * scale;
                }
                polygonCollider.points = newPoints;
            }
            else
            {
                Debug.LogWarning($" # Type of collider -> {collider.GetType()} is not supported for scaling!");
            }
        }
    }
}