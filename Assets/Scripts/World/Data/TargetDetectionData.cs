
using UnityEngine;

namespace World.Components.TargetDetection
{
    [CreateAssetMenu(fileName = "NewTargetDetectionData", menuName = "Game/World/Data/TargetDetectionData")]
    public class TargetDetectionData : ScriptableObject
    {
        [SerializeField] private TargetDetectionSettings _settings;

        public TargetDetectionSettings Settings => _settings;
    }
}
