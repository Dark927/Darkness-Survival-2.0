
using Characters.Common.Features;
using UnityEngine;

namespace Characters.Common.Abilities
{
    [CreateAssetMenu(fileName = "NewEntityAbilityData", menuName = "Game/Characters/Abilities/Entity Ability Data")]
    public class EntityAbilityData : FeatureData, IAbilityData
    {
        [SerializeField] private AbilityStats _abilityStats;

        [Header("UI Text Overrides")]
        [SerializeField] private AbilityStatsUI _abilityStatsUIOverrides;
        public AbilityStats AbilityStats => _abilityStats;
        public AbilityStatsUI AbilityStatsUI => _abilityStatsUIOverrides;
    }
}
