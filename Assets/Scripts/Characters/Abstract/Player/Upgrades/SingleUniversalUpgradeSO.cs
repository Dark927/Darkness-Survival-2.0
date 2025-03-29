
namespace Characters.Player.Upgrades
{
    /// <summary>
    /// The base class for all upgrades where should be some stats, etc (with the Downgrade feature).
    /// </summary>
    public abstract class SingleUniversalUpgradeSO<TUpgradeTarget> : SingleUpgradeBaseSO<TUpgradeTarget> where TUpgradeTarget : IUpgradable
    {
        public virtual string GetDowngradeInfo()
        {
            return GetInfo('-');
        }

        public abstract void ApplyDowngrade(TUpgradeTarget target);
    }
}
