using UnityEngine;

namespace Gameplay.Components.TargetDetection
{
    [System.Serializable]
    public struct TargetDetectionSettings
    {
        #region Fields

        public float Distance;
        public float AreaWidth;

        [Tooltip("Select the layers to detect (e.g., Enemy, Boss)")]
        public LayerMask LayerMask;

        #endregion

        #region Methods

        public TargetDetectionSettings(float distance, float areaWidth, int layerMask = Physics2D.DefaultRaycastLayers)
        {
            Distance = distance;
            AreaWidth = areaWidth;
            LayerMask = layerMask;
        }

        #endregion
    }
}
