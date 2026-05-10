namespace Characters.Player.Upgrades
{
    /// <summary>
    /// Marker interface used exclusively to group all Upgrade Levels that target Weapons.
    /// Prevents Abilities or Character-specific levels from being assigned to Weapons SOs.
    /// </summary>
    public interface IWeaponUpgradeLevelData : IUpgradeLevelData
    {
    }
}
