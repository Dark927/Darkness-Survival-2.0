namespace Characters.Player.Upgrades
{
    /// <summary>
    /// Marker interface used exclusively to group all Upgrade Levels that target the Character.
    /// Prevents Weapon or Ability-specific levels from being assigned to Character SOs.
    /// </summary>
    public interface ICharacterUpgradeLevelData : IUpgradeLevelData
    {
    }
}
