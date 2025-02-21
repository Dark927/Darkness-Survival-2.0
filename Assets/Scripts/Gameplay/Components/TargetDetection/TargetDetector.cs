using UnityEngine;
using Gameplay.Data;

namespace Gameplay.Components.TargetDetection
{
    public class TargetDetector
    {
        #region Fields 

        private Transform _sourceTransform;
        private Collider2D _sourceCollider;
        private TargetDetectionSettings _settings;

        #endregion


        #region Properties 

        public Vector3 SourceCenter => _sourceCollider.bounds.center;

        #endregion


        #region Methods

        #region Init

        public TargetDetector(Transform sourceTransform, TargetDetectionSettings settings)
        {
            _sourceTransform = sourceTransform;
            _sourceCollider = sourceTransform.GetComponent<Collider2D>();

            _settings = settings;
        }

        #endregion


        // ---------------------
        // Direct Searching
        // ---------------------

        // ToDo : Implement direct searching using default raycast without rectangle pattern.

        public bool IsTargetFoundByDirection<T>(Vector2 direction) where T : MonoBehaviour
        {
            return IsTargetFoundInBox<T>(direction, _settings);
        }

        public bool IsTargetFoundByDirection<T>(Vector2 direction, TargetDetectionSettings settings) where T : MonoBehaviour
        {
            return IsTargetFoundInBox<T>(direction, settings);
        }

        // Search On the Vertical Axis 

        public bool IsTargetFoundOnVerticalAxis<T>() where T : MonoBehaviour
        {
            return IsTargetFoundInBox<T>(Vector2.up, _settings) || IsTargetFoundInBox<T>(Vector2.down, _settings);
        }

        public bool IsTargetFoundOnVerticalAxis<T>(TargetDetectionSettings settings) where T : MonoBehaviour
        {
            return IsTargetFoundInBox<T>(Vector2.up, settings) || IsTargetFoundInBox<T>(Vector2.down, settings);
        }

        public bool IsTargetFoundOnVerticalAxis(Transform target)
        {
            return IsTargetFoundInBox(target.position, Vector2.up, _settings) || IsTargetFoundInBox(target.position, Vector2.down, _settings);
        }

        public bool IsTargetFoundOnVerticalAxis(Transform target, TargetDetectionSettings settings)
        {
            return IsTargetFoundInBox(target.position, Vector2.up, settings) || IsTargetFoundInBox(target.position, Vector2.down, settings);
        }

        // Search In the Box

        private bool IsTargetFoundInBox(Vector3 targetPosition, Vector2 direction, TargetDetectionSettings settings)
        {
            bool isFound = false;
            float distanceSqr = Mathf.Abs((targetPosition - SourceCenter).sqrMagnitude);
            float searchDistanceSqr = settings.Distance * settings.Distance;

            if (distanceSqr < searchDistanceSqr)
            {
                isFound = SearchTarget(targetPosition, direction, settings.AreaWidth, SourceCenter);
            }

#if UNITY_EDITOR

            Vector2 boxCenter = SourceCenter;
            DrawDebugRectangle(settings.Distance * direction.y, settings.AreaWidth, isFound, boxCenter);
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

        private bool IsTargetFoundInBox<T>(Vector2 direction, TargetDetectionSettings settings) where T : MonoBehaviour
        {
            bool isFound = false;
            float halfDistance = settings.Distance / 2f;

            Vector2 boxCenter = SourceCenter + (Vector3)(direction * halfDistance);
            Vector2 boxSize = new Vector2(settings.AreaWidth, settings.Distance);


            Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0, settings.LayerIndex);

            foreach (var hit in hits)
            {
                if (!ReferenceEquals(hit.GetComponent<T>(), null))
                {
                    isFound = true;
                    break;
                }
            }

#if UNITY_EDITOR

            DrawDebugRectangle(settings.Distance * direction.y, settings.AreaWidth, isFound, SourceCenter);
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

}
