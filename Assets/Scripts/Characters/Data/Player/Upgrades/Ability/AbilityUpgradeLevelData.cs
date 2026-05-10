using System;

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// The concrete class designers will select from the dropdown.
    /// </summary>
    [Serializable]
    public class AbilityUpgradeLevelData : UpgradeLevelData<IUpgradableAbility>, IAbilityUpgradeLevelData
    {
    }
}
