using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Utilities.Attributes;

namespace Characters.Player.Upgrades
{
    public abstract class UpgradeLevelSO<TTarget> : ScriptableObject, IUpgradeLevelSO where TTarget : IUpgradable
    {
        [CustomHeader("Main Settings", 9, 0)]
        [SerializeField, TextArea] private string _baseDescription;
        [SerializeField] private bool _useDowngradesInfoInDesc = true;

        [Space, SerializeField] private List<SingleUpgradeBaseSO<TTarget>> _upgrades;
        [SerializeField] private List<SingleUniversalUpgradeSO<TTarget>> _downgrades;

        [CustomHeader("Extra Settings (optional)", 3, 0)]

        [SerializeField] private Color _baseDescriptionColor = new Color(255, 230, 0, 255);
        [SerializeField] private UpgradeVisualDataUI _customVisualDataUI;
        [SerializeField] private Sprite _customIconUI;
        public string Description => GetDescription();
        public IEnumerable<SingleUpgradeBaseSO<TTarget>> Upgrades => _upgrades;
        public IEnumerable<SingleUniversalUpgradeSO<TTarget>> Downgrades => _downgrades;
        public UpgradeVisualDataUI CustomUpgradeVisualDataUI => _customVisualDataUI;
        public Sprite CustomIconUI => _customIconUI;

        public virtual string GetDescription()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(_baseDescription))
            {
                string colorHex = ColorUtility.ToHtmlStringRGBA(_baseDescriptionColor);
                string finalDescription = $"<color=#{colorHex}>{_baseDescription}</color>";
                sb.AppendLine(finalDescription);
            }

            string currentInfo;

            foreach (var upgrade in _upgrades)
            {
                currentInfo = upgrade.GetUpgradeInfo();
                if (!string.IsNullOrEmpty(currentInfo))
                {
                    sb.AppendLine(currentInfo);
                }
            }

            if (_useDowngradesInfoInDesc)
            {
                foreach (var downgrade in _downgrades)
                {
                    currentInfo = downgrade.GetDowngradeInfo();
                    if (!string.IsNullOrEmpty(currentInfo))
                    {
                        sb.AppendLine(currentInfo);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
