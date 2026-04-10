
using UnityEngine;

namespace Gameplay.Components
{
    [System.Serializable]
    public struct ObjectPoolSettings
    {
        [Header("Main Settings")]

        public const short NotIdentifiedPreloadCount = -1;

        [SerializeField] private int _preloadInstancesCount;
        [SerializeField] private int _maxPoolCapacity;

        public ObjectPoolSettings(int maxPoolCapacity, int preloadInstancesCount)
        {
            _preloadInstancesCount = preloadInstancesCount;
            _maxPoolCapacity = maxPoolCapacity;
        }

        public int PreloadInstancesCount => _preloadInstancesCount;
        public int MaxPoolCapacity => _maxPoolCapacity;
    }
}
