using UnityEngine;
using Utilities.Attributes;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class VacuumHazardAttackSettings : SlowingHazardAttackSettings
    {
        [Space, CustomHeader("Vaccum Moving Hazard Settings", 7, 1, CustomHeaderAttribute.HeaderColor.green)]
        [Header("Vacuum Physics")]
        [SerializeField] private float _pullStrength = 5f;
        public float PullStrength { get => _pullStrength; set => _pullStrength = value; }

        [SerializeField] private float _pullRadius = 5f;
        public float PullRadius { get => _pullRadius; set => _pullRadius = value; }

        [Header("Mobility")]
        [SerializeField] private float _moveSpeed = 3f;
        public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

        public VacuumHazardAttackSettings(VacuumHazardAttackSettings source) : base(source)
        {
            _pullStrength = source.PullStrength;
            _pullRadius = source.PullRadius;
            _moveSpeed = source.MoveSpeed;
        }

        public override IAttackSettings Clone()
        {
            return new VacuumHazardAttackSettings(this);
        }
    }
}
