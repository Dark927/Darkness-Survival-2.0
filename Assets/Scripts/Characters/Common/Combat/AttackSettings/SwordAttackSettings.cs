using Characters.Common.Combat.Weapons.Data;
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
        public float TriggerActivityTimeSec => _commonSettings.TriggerActivityTimeSec;
        public float FullDurationTimeSec => _commonSettings.FullDurationTimeSec;
        public float ReloadTimeSec => _commonSettings.ReloadTimeSec;
    }
}
