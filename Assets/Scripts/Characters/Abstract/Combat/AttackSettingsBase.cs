using Characters.Common.Combat.Weapons.Data;
using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class AttackSettingsBase
    {
        [Space, Header("Main Settings")]

        [SerializeField] private AttackNegativeStatusData _attackNegativeStatus;
        [SerializeField] private DamageSettings _damageSettings;
        [Space]
        [SerializeField] private ImpactSettings _impactSettings;


        [Space, Header("Activation Settings")]

        [SerializeField] private float _triggerActivityTimeSec;
        [SerializeField] private float _fullDurationTimeSec;
        [SerializeField] private float _reloadTimeSec;

        public DamageSettings Damage => _damageSettings;
        public ImpactSettings ImpactSettings => _impactSettings;
        public float TriggerActivityTimeSec => _triggerActivityTimeSec;
        public float FullDurationTimeSec => _fullDurationTimeSec;
        public float ReloadTimeSec => _reloadTimeSec;

    }
}
