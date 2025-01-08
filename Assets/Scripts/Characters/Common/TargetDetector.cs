using UnityEngine;


public class TargetDetector
{
    #region Fields 

    private Transform _sourceTransform;
    private Collider2D _sourceCollider;

    #endregion


    #region Methods

    public TargetDetector(Transform source)
    {
        _sourceTransform = source;
        _sourceCollider = _sourceTransform.GetComponent<Collider2D>();
    }

    public bool IsTargetFoundByDirection<T>(Vector2 direction,
                                            float distance,
                                            float areaWidth,
                                            int layerIndex = Physics2D.DefaultRaycastLayers
                                            ) where T : MonoBehaviour
    {
        // ToDo : Implement direct searching using default raycast without rectangle pattern.
        return IsTargetFoundInBox<T>(direction, distance, areaWidth, layerIndex);
    }

    public bool IsTargetFoundOnVerticalAxis<T>(float halfDistance,
                                               float areaWidth,
                                               int layerIndex = Physics2D.DefaultRaycastLayers
                                               ) where T : MonoBehaviour
    {
        return IsTargetFoundInBox<T>(Vector2.up, halfDistance, areaWidth, layerIndex) || IsTargetFoundInBox<T>(Vector2.down, halfDistance, areaWidth, layerIndex);
    }

    public bool IsTargetFoundOnVerticalAxis(Vector3 targetPosition,
                                            float halfDistance,
                                            float areaWidth
                                            )
    {
        return IsTargetFoundInBox(targetPosition, Vector2.up, halfDistance, areaWidth) || IsTargetFoundInBox(targetPosition, Vector2.down, halfDistance, areaWidth);
    }

    private bool IsTargetFoundInBox(Vector3 targetPosition, Vector2 direction, float searchDistance, float areaWidth)
    {
        Vector3 sourceCenter = _sourceCollider.bounds.center;

        bool isFound = false;
        float distanceSqr = Mathf.Abs((targetPosition - sourceCenter).sqrMagnitude);
        float searchDistanceSqr = searchDistance * searchDistance;

        if (distanceSqr < searchDistanceSqr)
        {
            isFound = SearchTarget(targetPosition, direction, areaWidth, sourceCenter);
        }

#if UNITY_EDITOR

        Vector2 boxCenter = sourceCenter;
        DrawDebugRectangle(searchDistance * direction.y, areaWidth, isFound, boxCenter);
#endif

        return isFound;
    }

    private bool SearchTarget(Vector3 targetPosition, Vector2 direction, float areaWidth, Vector3 sourceCenter)
    {
        bool isFound = false;

        // Check if the target direction matches the specified search direction

        float directionDiffSignY = Mathf.Sign(targetPosition.y - _sourceTransform.position.y);

        if (directionDiffSignY == Mathf.Sign(direction.y))
        {
            bool targetInsideLeftBound = (targetPosition.x > (sourceCenter.x - (areaWidth / 2f)));
            bool targetInsideRightBound = (targetPosition.x < (sourceCenter.x + (areaWidth / 2f)));
            isFound = targetInsideLeftBound && targetInsideRightBound;
        }

        return isFound;
    }

    private bool IsTargetFoundInBox<T>(Vector2 direction, float distance, float areaWidth, int layerIndex) where T : MonoBehaviour
    {
        Vector3 sourceCenter = _sourceCollider.bounds.center;
        bool isFound = false;
        float halfDistance = distance / 2f;

        Vector2 boxCenter = sourceCenter + (Vector3)(direction * halfDistance);
        Vector2 boxSize = new Vector2(areaWidth, distance);


        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0, layerIndex);

        foreach (var hit in hits)
        {
            if (!ReferenceEquals(hit.GetComponent<T>(), null))
            {
                isFound = true;
                break;
            }
        }

#if UNITY_EDITOR

        DrawDebugRectangle(distance * direction.y, areaWidth, isFound, sourceCenter);
#endif

        return isFound;

    }


#if UNITY_EDITOR

    /// <summary>
    /// Draw target detection debug rectangle using lines
    /// </summary>
    /// <param name="distance">max distance of detection (can be negative)</param>
    /// <param name="areaWidth">max width of detection</param>
    /// <param name="isFound">determines whether the object is found in the area</param>
    /// <param name="startPosition">start position for drawing a rectangle</param>
    private static void DrawDebugRectangle(float distance, float areaWidth, bool isFound, Vector2 startPosition)
    {
        Color debugColor = isFound ? Color.green : Color.red;
        float halfAreaWidth = areaWidth / 2f;

        Vector2 mainLineStart = startPosition + new Vector2(-halfAreaWidth, 0);
        Vector2 leftLineStart = startPosition + new Vector2(-halfAreaWidth, 0);
        Vector2 rightLineStart = startPosition + new Vector2(halfAreaWidth, 0);
        Vector2 oppositeLineStart = startPosition + new Vector2(-halfAreaWidth, distance);

        Debug.DrawLine(mainLineStart, mainLineStart + (Vector2.right * areaWidth), debugColor);
        Debug.DrawLine(leftLineStart, leftLineStart + (Vector2.up * distance), debugColor);
        Debug.DrawLine(rightLineStart, rightLineStart + (Vector2.up * distance), debugColor);
        Debug.DrawLine(oppositeLineStart, oppositeLineStart + (Vector2.right * areaWidth), debugColor);
    }

#endif

    #endregion
}

