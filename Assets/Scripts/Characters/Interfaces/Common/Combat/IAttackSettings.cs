
using Characters.Common.Combat.Weapons;

namespace Characters.Common.Combat
{
    /// <summary>
    /// A general interface for AttackSettings, which by default should be structures of type value.
    /// </summary>
    public interface IAttackSettings
    {
        public AttackNegativeStatusData NegativeStatus { get; set; }
        public DamageSettings Damage { get; set; }
        public ImpactSettings Impact { get; set; }
        public float TriggerActivityTimeSec { get; set; }
        public float FullDurationTimeSec { get; set; }
        public float ReloadTimeSec { get; set; }
    }
}
