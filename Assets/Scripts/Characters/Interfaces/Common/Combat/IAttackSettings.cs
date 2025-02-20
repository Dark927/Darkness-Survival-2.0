
using Characters.Common.Combat.Weapons.Data;
using UnityEngine;

namespace Characters.Common.Combat
{
    public interface IAttackSettings
    {
        public AttackNegativeStatusData NegativeStatus { get; }
        public DamageSettings Damage { get; }
        public ImpactSettings Impact { get; }
        public float TriggerActivityTimeSec { get; }
        public float FullDurationTimeSec { get; }
        public float ReloadTimeSec { get; }
    }
}
