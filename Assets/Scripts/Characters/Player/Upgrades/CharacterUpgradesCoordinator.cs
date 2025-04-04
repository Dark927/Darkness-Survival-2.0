using Characters.Common.Combat.Weapons;
using Characters.Common.Features;
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

        public void ApplyPassiveAbilityUpgrade(int abilityID, UpgradeLevelSO<IUpgradableAbility> upgradeLevel)
        {
            ApplyAbilityUpgrade(_character.AbilitiesHandler, abilityID, upgradeLevel);
        }

        public void ApplyWeaponAbilityUpgrade(int weaponID, UpgradeLevelSO<IUpgradableWeapon> upgradeLevel)
        {

            ApplyAbilityUpgrade(_character.WeaponsHandler, weaponID, upgradeLevel);
        }

        private void ApplyAbilityUpgrade<TSearchTarget, TUpgradableTarget>(IFeaturesHolderProvider<TSearchTarget> abilitiesProvider, int targetID, UpgradeLevelSO<TUpgradableTarget> upgradeLevel)
            where TUpgradableTarget : IUpgradable
        {
            if (!abilitiesProvider.TryGetFeatureByID(targetID, out var ability))
            {
                ErrorLogger.LogWarning($"Warning | No ability with ID == {targetID} found in collection | Upgrade will be ignored | {_character}");
                return;
            }

            if (ability is not TUpgradableTarget upgradableAbility)
            {
                ErrorLogger.LogWarning($"Warning | Ability ID == {targetID} is not of the expected type in collection | Upgrade will be ignored | {_character}");
                return;
            }

            ApplyUpgradeToTheTarget(upgradableAbility, upgradeLevel);
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
