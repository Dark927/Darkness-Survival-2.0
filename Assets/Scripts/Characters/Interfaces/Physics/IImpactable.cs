

namespace Characters.Common.Physics2D
{
    public interface IImpactable
    {
        public bool IsImmune { get; }
        public int ImmunityTimeMs { get; }
    }
}
