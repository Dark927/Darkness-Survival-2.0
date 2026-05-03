
namespace Characters.Common.Combat.Weapons
{
    public abstract class UpgradableWeaponBase : WeaponBase, IUpgradableWeapon
    {
        #region Fields

        private float _damageMultiplier = 1f;
        private float _reloadTimeMultiplier = 1f;
        private int _stunChancePercent = 0;
        private float _attackSpeedMultiplier = 1f;

        #endregion

        #region Properties
        protected float CurrentReloadTime => AttackSettings.ReloadTimeSec * _reloadTimeMultiplier;
        protected float CurrentAttackSpeedMultiplier => _attackSpeedMultiplier;

        #endregion


        #region Methods

        protected override float CalculateCurrentDamage(DamageSettings damageSettings)
        {
            return  base.CalculateCurrentDamage(damageSettings) * _damageMultiplier;
        }

        protected override bool CanUseImpactThisTime(AttackImpact activeImpact)
        {
            return ImpactAvailable && activeImpact != null && activeImpact.IsReady && CanUseImpactWithChance(activeImpact.ChancePercent + _stunChancePercent);
        }

        public virtual void ApplyDamageUpgrade(float multiplier)
        {
            _damageMultiplier += multiplier;
        }

        public virtual void ApplyAttackSpeedUpgrade(float multiplier)
        {
            _attackSpeedMultiplier += multiplier;
        }

        public void ApplyReloadSpeedUpgrade(float multiplier)
        {
            _reloadTimeMultiplier += multiplier;
        }

        public void ApplyStunChanceUpgrade(int percent)
        {
            _stunChancePercent += percent;
        }

        #endregion

    }
}
