

namespace Characters.Common.CustomPhysics2D
{
    public interface IImpactable
    {
        public bool IsImmune { get; }
        public int ImmunityTimeMs { get; }
    }
}
