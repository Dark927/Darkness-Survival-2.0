using System.Collections.Generic;
using System.Text;
using ModestTree;
using UI.Characters.Upgrades;
using UnityEngine;
using Utilities.Attributes;

namespace Characters.Player.Upgrades
{
    public abstract class UpgradeLevelSO<TTarget> : ScriptableObject, IUpgradeLevelSO where TTarget : IUpgradable
    {
        [CustomHeader("Main Settings", 5, 0)]
        [SerializeField, TextArea] private string _baseDescription;
        [SerializeField] private bool _useDowngradesInfoInDesc = true;

        [Space, SerializeField] private List<SingleUpgradeBaseSO<TTarget>> _upgrades;
        [SerializeField] private List<SingleUniversalUpgradeSO<TTarget>> _downgrades;

        [CustomHeader("Extra Settings (optional)", 1, 0)]

        [SerializeField] private UpgradeVisualDataUI _customVisualDataUI;

        public string Description => GetDescription();
        public IEnumerable<SingleUpgradeBaseSO<TTarget>> Upgrades => _upgrades;
        public IEnumerable<SingleUniversalUpgradeSO<TTarget>> Downgrades => _downgrades;
        public UpgradeVisualDataUI CustomUpgradeVisualDataUI => _customVisualDataUI;

        public virtual string GetDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(_baseDescription);

            string currentInfo;

            foreach (var upgrade in _upgrades)
            {
                currentInfo = upgrade.GetUpgradeInfo();
                if (!currentInfo.IsEmpty())
                {
                    sb.AppendLine(currentInfo);
                }
            }

            if (_useDowngradesInfoInDesc)
            {
                foreach (var downgrade in _downgrades)
                {
                    currentInfo = downgrade.GetDowngradeInfo();
                    if (!currentInfo.IsEmpty())
                    {
                        sb.AppendLine(currentInfo);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
