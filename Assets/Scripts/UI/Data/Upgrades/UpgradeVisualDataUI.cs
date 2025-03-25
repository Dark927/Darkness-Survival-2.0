

using UI.Characters.Upgrades;
using UnityEngine;
using Utilities.Attributes;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewUpgradeVisualDataUI", menuName = "Game/Upgrades/Main/Upgrade Visual Data UI")]
    public class UpgradeVisualDataUI : ScriptableObject
    {
        [Space, CustomHeader("Card Settings", count: 2, depth: 1, CustomHeaderAttribute.HeaderColor.cyan)]

        [SerializeField] private Color _titleColor = Color.white;
        [SerializeField] private Color _cardColor = Color.white;

        [Space, CustomHeader("Icon Settings", count: 1, depth: 1, CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private Color _iconTint = Color.white;

        [Space, CustomHeader("Animations Settings", count: 1, depth: 1, CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private CardAnimationDataUI _animationsData;


        public Color IconTint => _iconTint;
        public Color TitleColor => _titleColor;
        public Color CardColor => _cardColor;
        public CardAnimationDataUI AnimationsData => _animationsData;
    }
}
