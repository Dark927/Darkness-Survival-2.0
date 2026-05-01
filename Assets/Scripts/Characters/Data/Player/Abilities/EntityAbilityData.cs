
using Characters.Common.Features;
using UnityEngine;

namespace Characters.Common.Abilities
{
    [CreateAssetMenu(fileName = "NewEntityAbilityData", menuName = "Game/Characters/Abilities/Entity Ability Data")]
    public class EntityAbilityData : FeatureData, IAbilityData
    {
        [SerializeField] private AbilityStats _abilityStats;
        public AbilityStats AbilityStats => _abilityStats;
    }
}
