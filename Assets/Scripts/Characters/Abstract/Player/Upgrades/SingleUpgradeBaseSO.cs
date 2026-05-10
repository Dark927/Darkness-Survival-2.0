namespace Characters.Player.Upgrades
{
    /// <summary>
    /// GENERIC BASE: For standard upgrades that ONLY upgrade stats.
    /// </summary>
    public abstract class SingleUpgradeBaseSO<TTarget> : AbstractUpgradeSO, IUpgradeApplicator<TTarget>
        where TTarget : IUpgradable
    {
        // Enforced by IUpgradeApplicator
        public abstract void ApplyUpgrade(TTarget target);
    }
}
