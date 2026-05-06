using Characters.Common.Combat.Weapons;
using UnityEngine;
using Utilities.Attributes;

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

        [CustomHeader("Activation Settings", "These parameters are related to weapons that can be activated for the certain duration and have the reload time",
            3, 1, CustomHeaderAttribute.HeaderColor.yellow)]
        [SerializeField] private float _triggerActivityTimeSec;
        [SerializeField] private float _fullDurationTimeSec;
        [SerializeField] private float _reloadTimeSec;

        public BasicAttackSettings() { }

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

        // Virtual allows derived classes to return their specific types
        public virtual IAttackSettings Clone()
        {
            return new BasicAttackSettings(this);
        }
    }
}
