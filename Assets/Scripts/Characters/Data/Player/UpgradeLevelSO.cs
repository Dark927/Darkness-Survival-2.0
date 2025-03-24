using System.Collections.Generic;
using System.Text;
using ModestTree;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    public abstract class UpgradeLevelSO<TTarget> : ScriptableObject, IUpgradeLevelSO
    {
        [SerializeField, TextArea] private string _baseDescription;
        [SerializeField] private bool _useDowngradesInfoInDesc = true;

        [Space, SerializeField] private List<SingleUpgradeBaseSO<TTarget>> _upgrades;

        [SerializeField] private List<SingleUniversalUpgradeSO<TTarget>> _downgrades;

        public string Description => GetDescription();
        public IEnumerable<SingleUpgradeBaseSO<TTarget>> Upgrades => _upgrades;
        public IEnumerable<SingleUniversalUpgradeSO<TTarget>> Downgrades => _downgrades;

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
