using System;
using UnityEngine;
using Utilities.Attributes;

namespace Characters.Common.Combat
{
    [Serializable]
    public class HazardAttackSettings : BurstAoeAttackSettings
    {
        [Space, CustomHeader("Hazard Spawn Rules", "Controls the random distance boundaries for spawning hazards.", 2, 1, CustomHeaderAttribute.HeaderColor.yellow)]

        [Tooltip("The minimum distance from the owner to spawn. Prevents spawning directly under the owner.")]
        [SerializeField, Min(0)] private float _minSpawnRadius = 0.5f;

        [Tooltip("The maximum distance from the owner to spawn.")]
        [SerializeField, Min(0.1f)] private float _maxSpawnRadius = 2.5f;

        public float MinSpawnRadius { get => _minSpawnRadius; set => _minSpawnRadius = value; }
        public float MaxSpawnRadius { get => _maxSpawnRadius; set => _maxSpawnRadius = value; }

        public HazardAttackSettings(HazardAttackSettings source) : base(source)
        {
            _minSpawnRadius = source.MinSpawnRadius;
            _maxSpawnRadius = source.MaxSpawnRadius;
        }

        public override IAttackSettings Clone()
        {
            return new HazardAttackSettings(this);
        }
    }
}
