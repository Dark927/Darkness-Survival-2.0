using Characters.Player.Upgrades;

namespace Characters.Player.Abilities
{
    public interface ISoulMagnetAbility : IUpgradableAbility
    {
        void ApplyXpBonusUpgrade(float percentage);
    }
}
