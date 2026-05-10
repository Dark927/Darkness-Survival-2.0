using Characters.Player.Upgrades;

public interface IUpgradeApplicator<in TTarget> where TTarget : IUpgradable
{
    void ApplyUpgrade(TTarget target);
}
