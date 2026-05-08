using UnityEngine;
using Utilities.Attributes;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class BurstAoeAttackSettings : AoeAttackSettings
    {
        [Space, CustomHeader("Burst Settings", "Parameters for weapons that spawn multiple entities",
            1, 1, CustomHeaderAttribute.HeaderColor.green)]
        [SerializeField, Min(1)] private int _spawnCount = 1;

        public BurstAoeAttackSettings(BurstAoeAttackSettings source) : base(source)
        {
            _spawnCount = source.SpawnCount;
        }

        public int SpawnCount { get => _spawnCount; set => _spawnCount = value; }

        public override IAttackSettings Clone()
        {
            return new BurstAoeAttackSettings(this);
        }
    }
}
