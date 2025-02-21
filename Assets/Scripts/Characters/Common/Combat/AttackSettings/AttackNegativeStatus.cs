using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public struct AttackNegativeStatus
    {
        public static AttackNegativeStatus Zero = new AttackNegativeStatus() {_effectColor = Color.white, _effectDurationInSec = 0f, _effectSpeedInSec = 0f };

        #region Fields

        [SerializeField] private Color _effectColor;
        [SerializeField] private float _effectDurationInSec;
        [SerializeField] private float _effectSpeedInSec;

        #endregion


        #region Properties

        public Color VisualColor => _effectColor;
        public float EffectDurationInSec => _effectDurationInSec;
        public float EffectSpeedInSec => _effectSpeedInSec;

        #endregion
    }
}
