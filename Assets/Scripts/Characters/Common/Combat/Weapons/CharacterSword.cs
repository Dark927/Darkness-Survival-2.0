using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Characters.Common.Combat.Weapons.Data;
using Utilities.ErrorHandling;

namespace Characters.Common.Combat.Weapons
{
    public class CharacterSword : BasicCharacterWeapon
    {
        #region Fields 

        private List<SwordAttackTrigger> _attackTriggers;
        private SwordAttackSettings _swordAttackSettings;

        private BasicAttack.LocalType _currentAttackType;
        private Dictionary<BasicAttack.LocalType, AttackImpact> _attackImpactDict = new();

        #endregion


        #region Properties
        public SwordAttackSettings Settings => _swordAttackSettings;

        #endregion


        #region Methods

        #region Init

        public override void Initialize(WeaponAttackDataBase attackData)
        {
            base.Initialize(attackData);
            _attackImpactDict = new();

            _swordAttackSettings = (SwordAttackSettings)InitialAttackSettings;


            _attackTriggers = GetComponentsInChildren<SwordAttackTrigger>().ToList();
            _attackTriggers.ForEach(attackTrigger => attackTrigger.Initialize());

            foreach (SwordAttackTrigger trigger in _attackTriggers)
            {
                trigger.OnTriggerEnter += HitTargetListener;
                trigger.OnTriggerDeactivation += TriggerDeactivationListener;

                InitSwordImpact(trigger.TargetAttackType);
            }
        }

        protected override IAttackSettings GetInitialHeavyAttackSettings()
        {
            SwordAttackSettings initialSettings = (SwordAttackSettings)InitialAttackSettings;
            IAttackSettings heavyAttackSettings = initialSettings;
            heavyAttackSettings.Damage = initialSettings.HeavyDamage;
            heavyAttackSettings.Impact = initialSettings.HeavyImpact;
            return heavyAttackSettings;
        }

        protected override void SetDefaultPosRelatedToOwner()
        {
            transform.position = Owner.Body.Transform.position;
        }

        private void InitSwordImpact(BasicAttack.LocalType targetAttackType)
        {
            // Dynamically assign impact based on type
            AttackImpact impact = (targetAttackType == BasicAttack.LocalType.Fast)
                ? BaseAttackImpact
                : base.InitImpact(Settings.HeavyImpact);

            _attackImpactDict.Add(targetAttackType, impact);
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (SwordAttackTrigger trigger in _attackTriggers)
            {
                trigger.OnTriggerEnter -= HitTargetListener;
                trigger.OnTriggerDeactivation -= TriggerDeactivationListener;
            }

            _attackImpactDict.Clear();
        }

        #endregion

        public void TriggerAttack(BasicAttack.LocalType attackType)
        {
            _currentAttackType = attackType;
            SwordAttackTrigger attackTrigger = _attackTriggers.FirstOrDefault(trigger => trigger.TargetAttackType == attackType);

            if (attackTrigger == null) return;

            // Dynamically pull the correct upgraded settings from our Dictionary
            var settings = UpgradedSettingsDict[attackType];

            //ErrorLogger.Log($"Trigger Activity Time: {settings.TriggerActivityTimeSec}");
            attackTrigger.Activate(settings.TriggerActivityTimeSec);

            GetImpact(attackType)?.Shake?.Activate();
        }

        protected override float RequestDamageAmount()
        {
            return _currentAttackType switch
            {
                BasicAttack.LocalType.Fast => CalculateUpgradedDamage(),
                BasicAttack.LocalType.Heavy => CalculateUpgradedHeavyDamage(),
                _ => throw new NotImplementedException()
            };
        }

        protected override AttackImpact GetActiveAttackImpact()
        {
            // The base class will automatically call this and use the correct dictionary impact
            return _attackImpactDict.GetValueOrDefault(_currentAttackType, null);
        }

        public override void ApplyImpactChanceUpgrade(float additionalPercent)
        {
            base.ApplyImpactChanceUpgrade(additionalPercent); // Fast attacks

            // Apply impact upgrade for heavy attacks
            if (_attackImpactDict.TryGetValue(BasicAttack.LocalType.Heavy, out var heavyAttackImpact))
            {
                var upgradedImpactSettings = heavyAttackImpact.CurrentSettings;
                upgradedImpactSettings.SetChancePercent(upgradedImpactSettings.ChancePercent + additionalPercent);
                heavyAttackImpact.SetImpactSettings(upgradedImpactSettings);
            }
        }

        private AttackImpact GetImpact(BasicAttack.LocalType attackType)
        {
            return _attackImpactDict.GetValueOrDefault(attackType, null);
        }

        private void TriggerDeactivationListener(IAttackTrigger trigger)
        {
            SwordAttackTrigger swordAttackTrigger = (SwordAttackTrigger)trigger;

            if (_attackImpactDict.TryGetValue(swordAttackTrigger.TargetAttackType, out AttackImpact impact))
            {
                impact.ReloadImpact();
            }
        }

        #endregion
    }
}
