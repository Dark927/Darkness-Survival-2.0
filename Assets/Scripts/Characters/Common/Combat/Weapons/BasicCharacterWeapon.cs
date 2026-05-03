

using System.Collections.Generic;
using Characters.Common.Combat.Weapons.Data;
using Characters.Player;

namespace Characters.Common.Combat.Weapons
{
    public class BasicCharacterWeapon : UpgradableWeaponBase
    {
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
        }


        public override void ApplyAttackSpeedUpgrade(float multiplier)
        {
            ApplyConcreteAttackSpeedUpgrade(BasicAttack.LocalType.Fast, multiplier);
            ApplyConcreteAttackSpeedUpgrade(BasicAttack.LocalType.Heavy, multiplier);
        }

        public void ApplyConcreteAttackSpeedUpgrade(BasicAttack.LocalType attackType, float multiplier)
        {
            if (_attackSpeedMultipliers.ContainsKey(attackType))
            {
                _attackSpeedMultipliers[attackType] += multiplier;
            }
            else
            {
                // Fallback just in case a new type gets added later
                _attackSpeedMultipliers[attackType] = 1f + multiplier;
            }

            // Send the final, mathematically correct total to the Animator
            OwnerVisual.PlayerAnimController.UpdateAttackSpeed(attackType, _attackSpeedMultipliers[attackType]);
        }
    }
}
