
using UnityEngine;

namespace Characters.TargetDetection
{
    [CreateAssetMenu(fileName = "NewTargetDetectionData", menuName = "Game/Characters/Data/TargetDetectionData")]
    public class TargetDetectionData : ScriptableObject
    {
        [SerializeField] private TargetDetectionSettings _settings;

        public TargetDetectionSettings Settings => _settings;
    }
}
