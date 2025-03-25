using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public struct DamageSettings
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        public float Min => _min;
        public float Max => _max;

        public static DamageSettings operator *(DamageSettings damageSettings, float multiplier)
        {
            return new DamageSettings()
            {
                _min = damageSettings.Min * multiplier,
                _max = damageSettings.Max * multiplier
            };
        }

        public static DamageSettings operator +(DamageSettings damageSettings, float value)
        {
            return new DamageSettings()
            {
                _min = damageSettings.Min + value,
                _max = damageSettings.Max + value
            };
        }
    }
}
