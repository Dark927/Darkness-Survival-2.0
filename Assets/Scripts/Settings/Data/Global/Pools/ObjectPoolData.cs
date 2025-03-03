using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "NewDefaultPoolSettings", menuName = "Game/Settings/Pool/Default Pool Data")]
    public class ObjectPoolData : ScriptableObject
    {
        [Header("Main Settings")]

        public const short NotIdentifiedPreloadCount = -1;

        [SerializeField] private int _preloadInstancesCount;
        [SerializeField] private int _maxPoolCapacity;

        public int PreloadInstancesCount => _preloadInstancesCount;
        public int MaxPoolCapacity => _maxPoolCapacity;
    }
}