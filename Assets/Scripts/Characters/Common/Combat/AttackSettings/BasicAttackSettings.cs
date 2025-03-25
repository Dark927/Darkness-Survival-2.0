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


        public AttackNegativeStatusData NegativeStatus { get => _attackNegativeStatus; set => _attackNegativeStatus = value; }
        public DamageSettings Damage { get => _damageSettings; set => _damageSettings = value; }
        public ImpactSettings Impact { get => _impact; set => _impact = value; }
        public float TriggerActivityTimeSec => _triggerActivityTimeSec;
        public float FullDurationTimeSec => _fullDurationTimeSec;
        public float ReloadTimeSec => _reloadTimeSec;
    }
}
