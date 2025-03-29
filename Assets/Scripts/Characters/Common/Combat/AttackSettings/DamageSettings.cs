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

        public DamageSettings(float min, float max)
        {
            _min = min;
            _max = max;
        }

        public static DamageSettings operator *(DamageSettings damageSettings, float multiplier)
        {
            return new DamageSettings(damageSettings.Min * multiplier, damageSettings.Max * multiplier);
        }

        public static DamageSettings operator +(DamageSettings damageSettings, float value)
        {
            return new DamageSettings(damageSettings.Min + value, damageSettings.Max + value);
        }
    }
}
