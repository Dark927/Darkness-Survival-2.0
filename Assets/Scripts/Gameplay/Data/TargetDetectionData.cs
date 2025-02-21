
using UnityEngine;

namespace Gameplay.Components.TargetDetection
{
    [CreateAssetMenu(fileName = "NewTargetDetectionData", menuName = "Game/World/Data/TargetDetectionData")]
    public class TargetDetectionData : ScriptableObject
    {
        [SerializeField] private TargetDetectionSettings _settings;

        public TargetDetectionSettings Settings => _settings;
    }
}
