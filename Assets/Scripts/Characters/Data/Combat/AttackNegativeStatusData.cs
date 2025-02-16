using UnityEngine;

namespace Characters.Common.Combat.Weapons.Data
{
    [CreateAssetMenu(fileName = "NewAttackNegativeStatusData", menuName = "Game/Combat/Data/Attacks/AttackNegativeStatusData")]
    public class AttackNegativeStatusData : ScriptableObject
    {
        #region Fields

        [Space, Header("Main Settings")]
        [SerializeField] private string _name;
        [SerializeField, TextArea(2, 3)] private string _description;

        [Space, Header("Effect Settings")]

        [SerializeField] private Color _effectColor;
        [SerializeField] private float _effectDurationInSec;
        [SerializeField] private float _effectSpeedInSec;

        #endregion


        #region Properties

        public string Name => _name;
        public string Description => _description;
        public Color VisualColor => _effectColor;
        public float EffectDurationInSec => _effectDurationInSec;
        public float EffectSpeedInSec => _effectSpeedInSec;

        #endregion
    }
}
