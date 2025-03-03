

using Settings.CameraManagement.Shake;
using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public struct ImpactSettings
    {
        [SerializeField] private bool _useImpact;
        [SerializeField] private int _stunDurationMs;
        [SerializeField] private float _pushForce;
        [Space]
        [SerializeField] private int _reloadTimeMs;
        [SerializeField, Range(0, 100)] private int _chancePercent;
        [Space]
        [SerializeField] private ShakeSettings _shakeSettings;

        public bool UseImpact => _useImpact;
        public int StunDurationMs => _stunDurationMs;
        public float PushForce => _pushForce;
        public int ReloadTimeMs => _reloadTimeMs;
        public int ChancePercent => _chancePercent;
        public ShakeSettings ShakeSettings => _shakeSettings;


        public ImpactSettings(bool useImpact = false, int stunDurationMs = 0, float pushForce = 0, int reloadTimeMs = 0, int chancePercent = 0)
        {
            _useImpact = useImpact;
            _stunDurationMs = stunDurationMs;
            _pushForce = pushForce;
            _reloadTimeMs = reloadTimeMs;
            _chancePercent = chancePercent;
            _shakeSettings = ShakeSettings.Default;
        }

        public void SetShake(ShakeSettings shakeSettings)
        {
            _shakeSettings = shakeSettings;
        }
    }
}
