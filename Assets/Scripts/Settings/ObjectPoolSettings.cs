
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "NewPoolSettings", menuName = "Game/Settings/Pool Settings")]
    public class ObjectPoolSettings : ScriptableObject
    {
        [Header("Main Settings")]

        [SerializeField] private int _preloadInstancesCount;
        [SerializeField] private int _maxPoolCapacity;

        public int PreloadInstancesCount => _preloadInstancesCount;
        public int MaxPoolCapacity => _maxPoolCapacity;
    }

}