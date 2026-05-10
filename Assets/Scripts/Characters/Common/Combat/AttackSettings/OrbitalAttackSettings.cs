using UnityEngine;
using Utilities.Attributes;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class OrbitalAttackSettings : BurstAoeAttackSettings
    {
        [Space, CustomHeader("Orbit Settings", "Parameters for orbital weapons", 1, 1, CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private float _baseOrbitSpeed = 180f;

        public OrbitalAttackSettings(OrbitalAttackSettings source) : base(source)
        {
            _baseOrbitSpeed = source.OrbitSpeed;
        }

        public float OrbitSpeed { get => _baseOrbitSpeed; set => _baseOrbitSpeed = value; }

        public override IAttackSettings Clone()
        {
            return new OrbitalAttackSettings(this);
        }
    }
}
