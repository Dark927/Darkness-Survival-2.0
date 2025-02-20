

using Settings.CameraManagement.Shake;
using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class ImpactSettings
    {
        [SerializeField] private bool _useImpact = false;
        [SerializeField] private int _stunDurationMs = 0;
        [SerializeField] private float _pushForce = 0f;
        [Space]
        [SerializeField] private int _reloadTimeMs = 0;
        [SerializeField, Range(0, 100)] private int _chancePercent = 0;
        [Space]
        [SerializeField] private ShakeSettings _shakeSettings;

        public bool UseImpact => _useImpact;
        public int StunDurationMs => _stunDurationMs;
        public float PushForce => _pushForce;
        public int ReloadTimeMs => _reloadTimeMs;
        public int ChancePercent => _chancePercent;
        public ShakeSettings ShakeSettings => _shakeSettings;
    }
}
