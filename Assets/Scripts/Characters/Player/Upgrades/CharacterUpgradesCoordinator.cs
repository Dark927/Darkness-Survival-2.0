
using System.Diagnostics;
using Characters.Common.Combat.Weapons;
using Utilities.ErrorHandling;

namespace Characters.Player.Upgrades
{
    public class CharacterUpgradesCoordinator
    {
        private IUpgradableCharacterLogic _character;

        public CharacterUpgradesCoordinator(IUpgradableCharacterLogic targetCharacter)
        {
            _character = targetCharacter;
        }

        public void ApplyCharacterUpgrade(UpgradeLevelSO<IUpgradableCharacterLogic> upgradeLevel)
        {
            ApplyUpgradeToTheTarget(_character, upgradeLevel);
        }

        public void UnlockAbility(WeaponUnlockLevelSO unlockAbilityUpgrade)
        {
            ApplyUpgradeToTheTarget(_character, unlockAbilityUpgrade);
        }

        public void ApplyWeaponUpgrade(int weaponID, UpgradeLevelSO<IUpgradableWeapon> upgradeLevel)
        {
            if (_character.Weapons.ActiveOnesDict.TryGetValue(weaponID, out var weapon))
            {
                if (weapon is IUpgradableWeapon upgradableWeapon)
                {
                    ApplyUpgradeToTheTarget(upgradableWeapon, upgradeLevel);
                }
            }
            else
            {
                ErrorLogger.LogWarning($" Warning | There are no weapon with ID == {weaponID} | Upgrade will be ignored | {_character}");
            }
        }

        private void ApplyUpgradeToTheTarget<TTarget>(TTarget target, UpgradeLevelSO<TTarget> upgradeLevel) where TTarget : IUpgradable
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
