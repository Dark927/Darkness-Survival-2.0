using System;
using UnityEngine;
using Utilities.Attributes;

namespace Characters.Common.Combat
{
    [Serializable]
    public class SlowingHazardAttackSettings : HazardAttackSettings
    {
        [Space, CustomHeader("Slowing Hazard Settings", "The speed multiplier applied to enemies inside the zone (e.g., 0.4 = 40% speed)", 1, 1, CustomHeaderAttribute.HeaderColor.green)]
        [SerializeField, Range(0.1f, 1f)] private float _slowMultiplier = 0.5f;

        public float SlowMultiplier { get => _slowMultiplier; set => _slowMultiplier = value; }

        public SlowingHazardAttackSettings(SlowingHazardAttackSettings source) : base(source)
        {
            _slowMultiplier = source.SlowMultiplier;
        }

        public override IAttackSettings Clone()
        {
            return new SlowingHazardAttackSettings(this);
        }
    }
}
