using Characters.Common.Combat.Weapons.Data;
using Utilities.ErrorHandling;

namespace Characters.Common.Combat.Weapons
{
    public abstract class UpgradableWeaponBase : WeaponBase, IUpgradableWeapon
    {
        #region Fields

        private float _reloadTimeMultiplier = 1f;
        private float _attackSpeedMultiplier = 1f;
        private float _activeDurationMultiplier = 1f;
        private float _damageMultiplier = 1f;
        private float _extraImpactChancePercent = 0;

        protected IAttackSettings UpgradedAttackSettings;

        #endregion

        #region Properties


        #endregion


        #region Methods

        public override void Initialize(WeaponAttackDataBase attackData)
        {
            base.Initialize(attackData);

            UpgradedAttackSettings = new BasicAttackSettings(InitialAttackSettings);
        }

        protected float CalculateUpgradedDamage()
        {
            return base.CalculateCurrentDamage(UpgradedAttackSettings.Damage);
        }

        protected override float CalculateCurrentDamage(DamageSettings damageSettings)
        {
            return base.CalculateCurrentDamage(damageSettings) * _damageMultiplier;
        }

        public virtual void ApplyDamageUpgrade(float multiplier)
        {
            _damageMultiplier += multiplier;

            var currentDamage = InitialAttackSettings.Damage; // we get inital damage for correct calculations
            currentDamage.SetMin(currentDamage.Min * _damageMultiplier);
            currentDamage.SetMax(currentDamage.Max * _damageMultiplier);
            UpgradedAttackSettings.Damage = currentDamage;

            //ErrorLogger.Log($"Upgraded Damage: {UpgradedAttackSettings.Damage.Min:0.##} - {UpgradedAttackSettings.Damage.Max:0.##}");
        }

        public virtual void ApplyAttackSpeedUpgrade(float multiplier)
        {
            _attackSpeedMultiplier += multiplier;
            // We do division because upgrade should decrease trigger activity time
            UpgradedAttackSettings.TriggerActivityTimeSec = InitialAttackSettings.TriggerActivityTimeSec / _attackSpeedMultiplier;

            //ErrorLogger.Log($"Upgraded Attack Speed (Trigger Time): {UpgradedAttackSettings.TriggerActivityTimeSec:0.##}s");
        }

        public void ApplyReloadSpeedUpgrade(float multiplier)
        {
            _reloadTimeMultiplier += multiplier;
            // We do division because upgrade should decrease reload speed
            UpgradedAttackSettings.ReloadTimeSec = InitialAttackSettings.ReloadTimeSec / _reloadTimeMultiplier;

            //ErrorLogger.Log($"Upgraded Reload Time: {UpgradedAttackSettings.ReloadTimeSec:0.##}s");
        }

        public virtual void ApplyImpactChanceUpgrade(float additionalPercent)
        {
            _extraImpactChancePercent += additionalPercent;
            UpgradedAttackSettings.Impact.SetChancePercent(InitialAttackSettings.Impact.ChancePercent + _extraImpactChancePercent);

            BaseAttackImpact.SetImpactSettings(UpgradedAttackSettings.Impact);

            //ErrorLogger.Log($"Upgraded Impact Chance: {UpgradedAttackSettings.Impact.ChancePercent}%");
        }

        public void ApplyActiveDurationUpgrade(float multiplier)
        {
            _activeDurationMultiplier += multiplier;
            UpgradedAttackSettings.FullDurationTimeSec = InitialAttackSettings.FullDurationTimeSec * _activeDurationMultiplier;

            //ErrorLogger.Log($"Upgraded Active Duration: {UpgradedAttackSettings.FullDurationTimeSec:0.##}s");
        }

        #endregion

    }
}
