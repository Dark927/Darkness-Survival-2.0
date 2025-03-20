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
    }
}
