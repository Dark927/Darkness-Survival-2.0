


using UnityEngine;

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// The base class for all upgrades where should be some stats, etc.
    /// </summary>
    public abstract class SingleUpgradeBaseSO<TUpgradeTarget> : ScriptableObject, ISingleUpgrade where TUpgradeTarget : IUpgradable
    {
        public virtual string GetUpgradeInfo()
        {
            return GetInfo('+');
        }

        protected virtual string GetInfo(char sign)
        {
            return string.Empty;
        }

        public abstract void ApplyUpgrade(TUpgradeTarget target);
    }
}


