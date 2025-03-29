

using Characters.Common.Combat.Weapons.Data;
using Characters.Player;

namespace Characters.Common.Combat.Weapons
{
    public class BasicCharacterWeapon : UpgradableWeaponBase
    {
        private PlayerCharacterVisual _ownerVisual;

        public PlayerCharacterVisual OwnerVisual => _ownerVisual;

        public override void Initialize(WeaponAttackDataBase attackData)
        {
            base.Initialize(attackData);
            _ownerVisual = Owner.Body.Visual as PlayerCharacterVisual;
        }


        public override void ApplyAttackSpeedUpgrade(float percent)
        {
            ApplyConcreteAttackSpeedUpgrade(BasicAttack.LocalType.Fast, percent);
            ApplyConcreteAttackSpeedUpgrade(BasicAttack.LocalType.Heavy, percent);
        }

        public void ApplyConcreteAttackSpeedUpgrade(BasicAttack.LocalType attackType, float percent)
        {
            OwnerVisual.PlayerAnimController.UpdateAttackSpeed(attackType, percent);
        }
    }
}
