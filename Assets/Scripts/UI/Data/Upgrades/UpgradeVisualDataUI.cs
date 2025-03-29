

using UI.Characters.Upgrades;
using UnityEngine;
using Utilities.Attributes;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewUpgradeVisualDataUI", menuName = "Game/Upgrades/Main/Upgrade Visual Data UI")]
    public class UpgradeVisualDataUI : ScriptableObject
    {
        [SerializeField] private UpgradeCardVisualSettings _mainVisualSettings;

        [Space, CustomHeader("Animations Settings", count: 1, depth: 1, CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private CardAnimationDataUI _animationsData;

        public UpgradeCardVisualSettings MainVisualSettings => _mainVisualSettings;
        public CardAnimationDataUI AnimationsData => _animationsData;
    }
}
