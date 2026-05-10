using System;

namespace Characters.Player.Upgrades
{
    // It targets the Character, because the Character is the entity receiving the new weapon!
    [Serializable]
    public class WeaponUnlockLevelData : UpgradeLevelData<IUpgradableCharacterLogic>, ICharacterUpgradeLevelData
    {
    }
}
