
namespace Characters.Player.Upgrades
{
    /// <summary>
    /// The base class for all upgrades where should be some stats, etc (with the Downgrade feature).
    /// </summary>
    public abstract class SingleUniversalUpgradeSO<TUpgradeTarget> : SingleUpgradeBaseSO<TUpgradeTarget>
    {
        public abstract void ApplyDowngrade(TUpgradeTarget target);
    }
}
