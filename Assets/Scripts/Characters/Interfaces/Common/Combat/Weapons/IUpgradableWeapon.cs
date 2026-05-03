using Characters.Player.Upgrades;

namespace Characters.Common.Combat.Weapons
{
    public interface IUpgradableWeapon : IWeapon, IUpgradable
    {
        public void ApplyDamageUpgrade(float multiplier);
        public void ApplyAttackSpeedUpgrade(float multiplier);
        public void ApplyReloadSpeedUpgrade(float multiplier);
        public void ApplyStunChanceUpgrade(int percent);
    }
}
