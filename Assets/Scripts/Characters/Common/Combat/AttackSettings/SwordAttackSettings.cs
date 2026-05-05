using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public struct SwordAttackSettings : IAttackSettings
    {
        [SerializeField] private BasicAttackSettings _commonSettings;


        [Space, Header("Heavy Attack Settings")]

        [SerializeField] private DamageSettings _heavyDamageSettings;
        [Space]
        [SerializeField] private ImpactSettings _heavyImpactSettings;

        public BasicAttackSettings CommonSettings => _commonSettings;
        public DamageSettings HeavyDamage => _heavyDamageSettings;
        public ImpactSettings HeavyImpact => _heavyImpactSettings;


        public AttackNegativeStatusData NegativeStatus { get => _commonSettings.NegativeStatus; set => _commonSettings.NegativeStatus = value; }
        public DamageSettings Damage { get => _commonSettings.Damage; set => _commonSettings.Damage = value; }
        public ImpactSettings Impact { get => _commonSettings.Impact; set => _commonSettings.Impact = value; }

        public float TriggerActivityTimeSec { get => _commonSettings.TriggerActivityTimeSec; set => _commonSettings.TriggerActivityTimeSec = value; }
        public float FullDurationTimeSec { get => _commonSettings.FullDurationTimeSec; set => _commonSettings.FullDurationTimeSec = value; }
        public float ReloadTimeSec { get => _commonSettings.ReloadTimeSec; set => _commonSettings.ReloadTimeSec = value; }
    }
}
