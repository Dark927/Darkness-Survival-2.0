namespace Characters.Player.Upgrades
{
    /// <summary>
    /// GENERIC UNIVERSAL: For upgrades that can be applied AND downgraded (curses, trade-offs, removals).
    /// </summary>
    public abstract class SingleUniversalUpgradeSO<TTarget> : SingleUpgradeBaseSO<TTarget>, IDowngradeApplicator<TTarget>
        where TTarget : IUpgradable
    {
        // Enforced by IDowngradeApplicator
        public abstract void ApplyDowngrade(TTarget target);
    }
}
