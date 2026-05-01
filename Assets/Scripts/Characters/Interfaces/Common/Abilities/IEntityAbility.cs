

using Characters.Common.Features;

namespace Characters.Common.Abilities
{
    public interface IEntityAbility : IEntityFeature
    {
        public void SetStaticStats(AbilityStats abilityStats);
    }
}
