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

        public void ApplyCharacterUpgrade(ICharacterUpgradeLevelData upgradeLevel)
        {
            upgradeLevel.ApplyTo(_character);
        }

        public void ApplyPassiveAbilityUpgrade(int abilityID, IUpgradeLevelData upgradeLevel)
        {
            ApplyAbilityUpgrade(_character.AbilitiesHandler, abilityID, upgradeLevel);
        }

        public void ApplyWeaponAbilityUpgrade(int weaponID, IUpgradeLevelData upgradeLevel)
        {
            ApplyAbilityUpgrade(_character.WeaponsHandler, weaponID, upgradeLevel);
        }

        private void ApplyAbilityUpgrade<TSearchTarget>(IFeaturesHolderProvider<TSearchTarget> abilitiesProvider, int targetID, IUpgradeLevelData upgradeLevel)
        {
            if (!abilitiesProvider.TryGetFeatureByID(targetID, out var ability))
            {
                ErrorLogger.LogWarning($"Warning | No ability with ID == {targetID} found in collection | Upgrade will be ignored | {_character}");
                return;
            }

            // If the feature is upgradable, pass it to the Level Data.
            // The Level Data will automatically verify the type
            if (ability is IUpgradable upgradableAbility)
            {
                upgradeLevel.ApplyTo(upgradableAbility);
            }
            else
            {
                ErrorLogger.LogWarning($"Warning | Ability ID == {targetID} is not IUpgradable | {_character}");
            }
        }
    }
}
