using UnityEngine;

namespace Gameplay.Components.TargetDetection
{
    [System.Serializable]
    public struct TargetDetectionSettings
    {

        #region Fields

        public float Distance;
        public float AreaWidth;
        public int LayerIndex;

        #endregion


        #region Methods

        public TargetDetectionSettings(float distance, float areaWidth, int layerIndex = Physics2D.DefaultRaycastLayers)
        {
            Distance = distance;
            AreaWidth = areaWidth;
            LayerIndex = layerIndex;
        }

        #endregion
    }
}
