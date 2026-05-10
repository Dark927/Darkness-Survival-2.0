using Characters.Player.Upgrades;

namespace Characters.Common.Combat.Weapons
{
    // Level 1: Every weapon has these
    public interface IUpgradableWeapon : IWeapon, IUpgradable
    {
        void ApplyDamageUpgrade(float multiplier);
        void ApplyAttackSpeedUpgrade(float multiplier);
        void ApplyReloadSpeedUpgrade(float multiplier);
        void ApplyActiveDurationUpgrade(float multiplier);
        void ApplyAttackRadiusUpgrade(float multiplier);
        void ApplyImpactChanceUpgrade(float additionalPercent);
    }

    // Level 2: Only weapons that spawn multiple things (Projectiles, Meteors)
    public interface IUpgradableBurstWeapon : IUpgradableWeapon
    {
        void ApplyAttackCountUpgrade(int additionalCount);
    }

    // Level 3: Only weapons that orbit the player (Sickles, Bibles)
    public interface IUpgradableOrbitalWeapon : IUpgradableBurstWeapon
    {
        void ApplyOrbitalSpeedUpgrade(float multiplier);
    }
}
