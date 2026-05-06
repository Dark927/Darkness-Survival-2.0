using UnityEngine;
using Utilities.Attributes;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class AoeAttackSettings : BasicAttackSettings, IAttackSettings
    {
        [Space, CustomHeader("AoE Settings", "These parameters are related to AoE weapons",
            1, 1, CustomHeaderAttribute.HeaderColor.yellow)]
        [SerializeField] private float _attackRadius;

        public AoeAttackSettings(AoeAttackSettings source) : base(source)
        {
            _attackRadius = source.AttackRadius;
        }

        public float AttackRadius { get => _attackRadius; set => _attackRadius = value; }

        // Overrides base clone to return the AoE specific instance
        public override IAttackSettings Clone()
        {
            return new AoeAttackSettings(this);
        }
    }
}
