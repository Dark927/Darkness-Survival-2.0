using Characters.Player.Upgrades;

public interface IDowngradeApplicator<in TTarget> where TTarget : IUpgradable
{
    void ApplyDowngrade(TTarget target);
}
