
using System.Linq;
using Characters.Common.Combat.Weapons;
using Characters.Interfaces;
using Utilities.ErrorHandling;

namespace Characters.Player.Upgrades
{
    public class CharacterUpgrades
    {
        private ICharacterLogic _character;

        public CharacterUpgrades(ICharacterLogic targetCharacter)
        {
            _character = targetCharacter;
        }

        public void ApplyCharacterUpgrade(UpgradeLevelSO<ICharacterLogic> upgradeLevel)
        {
            ApplyUpgradeToTheTarget(_character, upgradeLevel);
        }

        public void UnlockAbility(WeaponUnlockLevelSO unlockAbilityUpgrade)
        {
            ApplyUpgradeToTheTarget(_character, unlockAbilityUpgrade);
        }

        public void ApplyWeaponUpgrade(int weaponID, UpgradeLevelSO<IWeapon> upgradeLevel)
        {
            if (_character.Weapons.ActiveOnesDict.TryGetValue(weaponID, out var weapon))
            {
                ApplyUpgradeToTheTarget(weapon, upgradeLevel);
            }
            else
            {
                ErrorLogger.LogWarning($" Warning | There are no weapon with ID == {weaponID} | Upgrade will be ignored | {_character}");
            }
        }

        private void ApplyUpgradeToTheTarget<TTarget>(TTarget target, UpgradeLevelSO<TTarget> upgradeLevel)
        {
            foreach (var upgrade in upgradeLevel.Upgrades)
            {
                upgrade.ApplyUpgrade(target);
            }

            foreach (var upgrade in upgradeLevel.Downgrades)
            {
                upgrade.ApplyDowngrade(target);
            }
        }
    }
}
