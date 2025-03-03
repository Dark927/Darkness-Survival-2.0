using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public struct DamageSettings
    {
        [SerializeField] private float _minDamage;
        [SerializeField] private float _maxDamage;

        public float Min => _minDamage;
        public float Max => _maxDamage;
    }
}
