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

    // Level 3: Hazard Zones
    public interface IUpgradableHazardWeapon : IUpgradableBurstWeapon
    {
        void ApplyMinSpawnRadiusUpgrade(float multiplier);
        void ApplyMaxSpawnRadiusUpgrade(float multiplier);
    }

    // Status Modifiers (Can be applied to ANY weapon type)
    // For other Statuses create separate interfaces (!)
    public interface IUpgradableSlowingWeapon : IUpgradableWeapon
    {
        void ApplySlowStrengthUpgrade(float multiplier);
    }

    public interface IUpgradableVacuumWeapon : IUpgradableWeapon
    {
        void ApplyVacuumStrengthUpgrade(float multiplier);
        void ApplyVacuumRadiusUpgrade(float multiplier);
    }

    // For any weapon that physically travels across the screen
    public interface IUpgradableMobileWeapon : IUpgradableWeapon
    {
        void ApplyMovementSpeedUpgrade(float multiplier);
    }



    // Level 4: Complex Anomaly Weapons (Tornadoes, Black Holes, Blizzards)
    // Inherits all the modular traits needed for this weapon type
    public interface IUpgradableStormWeapon :
        IUpgradableHazardWeapon,   // For Min/Max Spawn Radius
        IUpgradableSlowingWeapon,  // For Slow Strength
        IUpgradableVacuumWeapon,   // For Pull Strength/Radius
        IUpgradableMobileWeapon    // For Travel Speed
    {

    }
}
