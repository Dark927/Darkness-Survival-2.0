using System.Collections.Generic;
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
        public List<StatUIInfo> UpgradeDetails => GetUpgradeDetails();
        public IEnumerable<SingleUpgradeBaseSO<TTarget>> Upgrades => _upgrades;
        public IEnumerable<SingleUniversalUpgradeSO<TTarget>> Downgrades => _downgrades;
        public UpgradeVisualDataUI CustomUpgradeVisualDataUI => _customVisualDataUI;
        public Sprite CustomIconUI => _customIconUI;

        public virtual List<StatUIInfo> GetUpgradeDetails()
        {
            List<StatUIInfo> allStats = new List<StatUIInfo>();

            // add base name
            if (!string.IsNullOrEmpty(_baseDescription))
            {
                allStats.Add(new StatUIInfo
                {
                    StatName = _baseDescription,
                    NameColor = _baseDescriptionColor,
                    FormatTemplate = "{0}" // template for single argument
                });
            }

            // get all upgrades
            foreach (var upgrade in _upgrades)
            {
                var upgradeInfoList = upgrade.GetUpgradeInfo();
                if (upgradeInfoList != null && upgradeInfoList.Count > 0)
                {
                    allStats.AddRange(upgradeInfoList);
                }
            }

            // get all downgrades
            if (_useDowngradesInfoInDesc)
            {
                foreach (var downgrade in _downgrades)
                {
                    var downgradeInfoList = downgrade.GetDowngradeInfo();
                    if (downgradeInfoList != null && downgradeInfoList.Count > 0)
                    {
                        allStats.AddRange(downgradeInfoList);
                    }
                }
            }

            return allStats;
        }
    }
}
