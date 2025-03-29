using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utilities.Attributes;

namespace Characters.Player.Upgrades
{
    [System.Serializable]
    public struct UpgradeCardVisualSettings
    {
        [Space, CustomHeader("Card Settings", count: 2, depth: 1, CustomHeaderAttribute.HeaderColor.cyan)]

        [SerializeField] private Color _titleColor;
        [SerializeField] private Color _cardColor;

        [Space, CustomHeader("Icon Settings", count: 1, depth: 1, CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private Color _iconTint;

        public UpgradeCardVisualSettings(Color titleColor, Color cardColor, Color iconTint)
        {
            _titleColor = titleColor;
            _cardColor = cardColor;
            _iconTint = iconTint;
        }

        public Color IconTint => _iconTint;
        public Color TitleColor => _titleColor;
        public Color CardColor => _cardColor;
    }
}
