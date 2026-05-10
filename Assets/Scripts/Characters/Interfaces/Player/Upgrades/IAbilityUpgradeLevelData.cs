namespace Characters.Player.Upgrades
{
    /// <summary>
    /// Marker interface used exclusively to group all Upgrade Levels that target Abilities.
    /// Prevents Weapon or Character-specific levels from being assigned to Ability SOs.
    /// </summary>
    public interface IAbilityUpgradeLevelData : IUpgradeLevelData
    {
    }
}
