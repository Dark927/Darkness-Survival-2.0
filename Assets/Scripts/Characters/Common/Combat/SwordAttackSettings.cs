using Settings.CameraManagement.Shake;
using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class SwordAttackSettings : AttackSettingsBase
    {
        [Space, Header("Impact Settings")]
        [SerializeField] private bool _hasImpact = false;
        [SerializeField] private ShakeSettings _fastShakeSettings;
        [SerializeField] private ShakeSettings _heavyShakeSettings;

        public bool HasImpact => _hasImpact;
        public ShakeSettings FastShakeSettings => _fastShakeSettings;
        public ShakeSettings HeavyShakeSettings => _heavyShakeSettings;
    }
}
