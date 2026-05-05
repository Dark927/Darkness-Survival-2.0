using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public struct BasicAttackSettings : IAttackSettings
    {
        [Space, Header("Main Settings")]

        [SerializeField] private AttackNegativeStatusData _attackNegativeStatus;
        [SerializeField] private DamageSettings _damageSettings;
        [Space]
        [SerializeField] private ImpactSettings _impact;


        [Space, Header("Activation Settings")]


        [Header("<color=yellow><i>These parameters are related to weapons" +
                "\nthat can be activated for the certain duration and have the reload time</i></color>")]
        [SerializeField] private float _triggerActivityTimeSec;
        [SerializeField] private float _fullDurationTimeSec;
        [SerializeField] private float _reloadTimeSec;

        public BasicAttackSettings(IAttackSettings source)
        {
            _attackNegativeStatus = source.NegativeStatus;
            _damageSettings = source.Damage;
            _impact = source.Impact;
            _triggerActivityTimeSec = source.TriggerActivityTimeSec;
            _fullDurationTimeSec = source.FullDurationTimeSec;
            _reloadTimeSec = source.ReloadTimeSec;
        }

        public AttackNegativeStatusData NegativeStatus { get => _attackNegativeStatus; set => _attackNegativeStatus = value; }
        public DamageSettings Damage { get => _damageSettings; set => _damageSettings = value; }
        public ImpactSettings Impact { get => _impact; set => _impact = value; }
        public float TriggerActivityTimeSec { get => _triggerActivityTimeSec; set => _triggerActivityTimeSec = value; }
        public float FullDurationTimeSec { get => _fullDurationTimeSec; set => _fullDurationTimeSec = value; }
        public float ReloadTimeSec { get => _reloadTimeSec; set => _reloadTimeSec = value; }
    }
}
