
namespace Characters.Common.Combat.Weapons
{
    public abstract class UpgradableWeaponBase : WeaponBase, IUpgradableWeapon
    {
        #region Fields


        #endregion

        #region Properties


        #endregion


        #region Methods

        public virtual void ApplyAttackSpeedUpgrade(float percent)
        {

        }

        public virtual void ApplyDamageUpgrade(float percent)
        {
            AttackSettings.Damage = AttackSettings.Damage * percent;
        }

        #endregion

    }
}
