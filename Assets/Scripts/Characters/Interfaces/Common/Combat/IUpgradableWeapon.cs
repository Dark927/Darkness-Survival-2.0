using Characters.Player.Upgrades;

namespace Characters.Common.Combat.Weapons
{
    public interface IUpgradableWeapon : IWeapon, IUpgradable
    {
        public void ApplyDamageUpgrade(float percent);
        public void ApplyAttackSpeedUpgrade(float percent);
    }
}
