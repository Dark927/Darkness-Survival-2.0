using Gameplay.Components;
using UnityEngine;

namespace Settings.Global.Audio
{
    [System.Serializable]
    public struct SoundsPlayerSettings
    {
        [Tooltip("A larger value provides a closer distance to the listener. Zero is farther away from the camera.")]
        [SerializeField] private ObjectPoolSettings _soundSourcesPoolSettings;
        [Space]
        [SerializeField, Range(0, 1)] private float _defaultVolume;
        [SerializeField, Range(-10, 0)] private float _offsetZ;
        [SerializeField, Range(0f, 1f)] private float _spatialBlend;    // 2D - 3D sound settings
        [SerializeField, Min(0.01f)] private float _minDistance;        // The distance at which the sound will be at its full volume
        [SerializeField] private float _maxDistance;                    // The maximum distance where the sound will fade out

        public ObjectPoolSettings SoundSourcesPoolSettings => _soundSourcesPoolSettings;
        public float DefaultVolume => _defaultVolume;
        public float SpatialBlend => _spatialBlend;
        public float MinDistance => _minDistance;
        public float MaxDistance => _maxDistance;
        public float PositionOffsetZ => _offsetZ;

    }
}
