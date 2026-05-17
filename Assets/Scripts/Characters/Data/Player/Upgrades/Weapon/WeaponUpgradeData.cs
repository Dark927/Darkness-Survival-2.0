using System;
using Characters.Common.Combat.Weapons;

namespace Characters.Player.Upgrades
{
    // The classes the designer will select from the dropdown menu
    [Serializable]
    public class CoreWeaponUpgradeData : UpgradeLevelData<IUpgradableWeapon>, IWeaponUpgradeLevelData { }

    [Serializable]
    public class BurstWeaponUpgradeData : UpgradeLevelData<IUpgradableBurstWeapon>, IWeaponUpgradeLevelData { }

    [Serializable]
    public class OrbitalWeaponUpgradeData : UpgradeLevelData<IUpgradableOrbitalWeapon>, IWeaponUpgradeLevelData { }

    [Serializable]
    public class StormWeaponUpgradeData : UpgradeLevelData<IUpgradableStormWeapon>, IWeaponUpgradeLevelData { }

    [Serializable]
    public class ReactiveWeaponUpgradeData : UpgradeLevelData<IUpgradableReactiveWeapon>, IWeaponUpgradeLevelData { }
}
