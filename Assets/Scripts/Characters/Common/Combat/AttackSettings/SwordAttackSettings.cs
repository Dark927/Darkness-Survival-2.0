using UnityEngine;
using Utilities.Attributes;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class SwordAttackSettings : BasicAttackSettings
    {
        [Space, CustomHeader("Heavy Attack Settings", 1, 1, CustomHeaderAttribute.HeaderColor.yellow)]
        [SerializeField] private DamageSettings _heavyDamageSettings;
        [Space]
        [SerializeField] private ImpactSettings _heavyImpactSettings;

        public SwordAttackSettings(SwordAttackSettings source) : base(source)
        {
            _heavyDamageSettings = source.HeavyDamage;
            _heavyImpactSettings = source.HeavyImpact;
        }

        public DamageSettings HeavyDamage { get => _heavyDamageSettings; set => _heavyDamageSettings = value; }
        public ImpactSettings HeavyImpact { get => _heavyImpactSettings; set => _heavyImpactSettings = value; }

        // Overrides base clone to return the Sword specific instance
        public override IAttackSettings Clone()
        {
            return new SwordAttackSettings(this);
        }
    }
}
