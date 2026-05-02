

using Characters.Common.Features;

namespace Characters.Common.Abilities
{
    public interface IAbilityData : IFeatureData
    {
        AbilityStats AbilityStats { get; }
    }
}
