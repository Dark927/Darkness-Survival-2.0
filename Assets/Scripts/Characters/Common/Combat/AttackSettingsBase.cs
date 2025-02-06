using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class AttackSettingsBase
    {
        [Space, Header("Damage Settings")]

        [SerializeField] private DamageSettings _damageSettings;

        [Space, Header("Activation Settings")]

        [SerializeField] private float _triggerActivityTimeSec;
        [SerializeField] private float _fullDurationTimeSec;
        [SerializeField] private float _reloadTimeSec;

        public DamageSettings Damage => _damageSettings;
        public float TriggerActivityTimeSec => _triggerActivityTimeSec;
        public float FullDurationTimeSec => _fullDurationTimeSec;
        public float ReloadTimeSec => _reloadTimeSec;  

    }
}
