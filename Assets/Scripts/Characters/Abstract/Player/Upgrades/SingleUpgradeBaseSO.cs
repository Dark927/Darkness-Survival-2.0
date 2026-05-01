


using ModestTree;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// The base class for all upgrades where should be some stats, etc.
    /// </summary>
    public abstract class SingleUpgradeBaseSO<TUpgradeTarget> : ScriptableObject, ISingleUpgrade where TUpgradeTarget : IUpgradable
    {
        [SerializeField] private string _upgradeStatNameUIOverride = null;

        protected string StatNameUI => (_upgradeStatNameUIOverride != null) && !_upgradeStatNameUIOverride.IsEmpty()
                                    ? _upgradeStatNameUIOverride
                                    : GetDefaultStatNameUI();

        public virtual string GetUpgradeInfo()
        {
            return GetInfo('+');
        }

        protected virtual string GetInfo(char sign)
        {
            return string.Empty;
        }

        protected virtual string GetDefaultStatNameUI()
        {
            return "DefaultStatName";
        }

        public abstract void ApplyUpgrade(TUpgradeTarget target);
    }
}


