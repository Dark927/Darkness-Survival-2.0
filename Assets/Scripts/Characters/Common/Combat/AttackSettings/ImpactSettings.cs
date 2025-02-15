

using Settings.CameraManagement.Shake;
using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class ImpactSettings
    {
        [SerializeField] private bool _useImpact = false;
        [SerializeField] private float _stunningDuration = 0f;
        [SerializeField] private float _pushForce = 0f;
        [Space]
        [SerializeField] private ShakeSettings _shakeSettings;

        public bool UseImpact => _useImpact;
        public float StunningDuration => _stunningDuration;
        public float PushForce => _pushForce;
        public ShakeSettings ShakeSettings => _shakeSettings;
    }
}
