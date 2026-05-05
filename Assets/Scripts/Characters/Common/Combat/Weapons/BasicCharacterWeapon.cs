using System.Collections.Generic;
using Characters.Common.Combat.Weapons.Data;
using Characters.Player;

namespace Characters.Common.Combat.Weapons
{
    public class BasicCharacterWeapon : UpgradableWeaponBase
    {
        protected Dictionary<BasicAttack.LocalType, IAttackSettings> InitialSettingsDict = new();
        protected Dictionary<BasicAttack.LocalType, IAttackSettings> UpgradedSettingsDict = new();

        private Dictionary<BasicAttack.LocalType, float> _attackSpeedMultipliers = new()
        {
            { BasicAttack.LocalType.Fast, 1f },
            { BasicAttack.LocalType.Heavy, 1f }
        };

        private PlayerCharacterVisual _ownerVisual;

        public PlayerCharacterVisual OwnerVisual => _ownerVisual;

        public override void Initialize(WeaponAttackDataBase attackData)
        {
            base.Initialize(attackData);
            _ownerVisual = Owner.Body.Visual as PlayerCharacterVisual;

            // Map Fast (Primary) Attack Settings
            InitialSettingsDict[BasicAttack.LocalType.Fast] = InitialAttackSettings;
            UpgradedSettingsDict[BasicAttack.LocalType.Fast] = UpgradedAttackSettings;

            // Map Heavy Attack Settings
            InitialSettingsDict[BasicAttack.LocalType.Heavy] = GetInitialHeavyAttackSettings();
            UpgradedSettingsDict[BasicAttack.LocalType.Heavy] = InitialSettingsDict[BasicAttack.LocalType.Heavy];
        }

        protected virtual IAttackSettings GetInitialHeavyAttackSettings()
        {
            return InitialAttackSettings;
        }

        protected float CalculateUpgradedHeavyDamage()
        {
            return base.CalculateCurrentDamage(UpgradedSettingsDict[BasicAttack.LocalType.Heavy].Damage);
        }

        public override void ApplyAttackSpeedUpgrade(float multiplier)
        {
            ApplyConcreteAttackSpeedUpgrade(BasicAttack.LocalType.Fast, multiplier);
            ApplyConcreteAttackSpeedUpgrade(BasicAttack.LocalType.Heavy, multiplier);
        }

        public void ApplyConcreteAttackSpeedUpgrade(BasicAttack.LocalType attackType, float multiplier)
        {
            // Ensure the dictionary has the type (fallback if not)
            _attackSpeedMultipliers.TryAdd(attackType, 1f);

            _attackSpeedMultipliers[attackType] += multiplier;

            // Keep base class logic in sync ONLY for the fast (primary) attack
            if (attackType == BasicAttack.LocalType.Fast)
            {
                base.ApplyAttackSpeedUpgrade(multiplier);
            }

            var upgradedSettings = UpgradedSettingsDict[attackType];
            upgradedSettings.TriggerActivityTimeSec = InitialSettingsDict[attackType].TriggerActivityTimeSec / _attackSpeedMultipliers[attackType];
            UpgradedSettingsDict[attackType] = upgradedSettings;

            OwnerVisual.PlayerAnimController.UpdateAttackSpeed(attackType, _attackSpeedMultipliers[attackType]);
        }
    }
}
