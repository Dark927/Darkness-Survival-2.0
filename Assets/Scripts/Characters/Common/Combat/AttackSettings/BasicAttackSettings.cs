using Characters.Common.Combat.Weapons.Data;
using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class BasicAttackSettings : IAttackSettings
    {
        [Space, Header("Main Settings")]

        [SerializeField] private AttackNegativeStatusData _attackNegativeStatus;
        [SerializeField] private DamageSettings _damageSettings;
        [Space]
        [SerializeField] private ImpactSettings _impact;


        [Space, Header("Activation Settings")]

        [SerializeField] private float _triggerActivityTimeSec;

        [Header("<color=yellow><i>These parameters are related to weapons" +
                "\nthat can be activated for the certain duration and have the reload time</i></color>")]
        [SerializeField] private float _fullDurationTimeSec;
        [SerializeField] private float _reloadTimeSec;

        public AttackNegativeStatusData NegativeStatus => _attackNegativeStatus;
        public DamageSettings Damage => _damageSettings;
        public ImpactSettings Impact => _impact;
        public float TriggerActivityTimeSec => _triggerActivityTimeSec;
        public float FullDurationTimeSec => _fullDurationTimeSec;
        public float ReloadTimeSec => _reloadTimeSec;

    }
}
