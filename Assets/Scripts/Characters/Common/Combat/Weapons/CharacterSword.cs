using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Common.Combat.Weapons.Data;

namespace Characters.Common.Combat.Weapons
{
    public class CharacterSword : BasicCharacterWeapon
    {
        #region Enums

        public enum AttackType
        {
            Fast = 1,
            Heavy = 2,
        }

        #endregion

        #region Fields 

        private List<SwordAttackTrigger> _attackTriggers;
        private SwordAttackSettings _swordAttackSettings;

        private AttackType _currentAttackType;
        private Dictionary<AttackType, AttackImpact> _attackImpactDict;

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

            _swordAttackSettings = (SwordAttackSettings)AttackSettings;
            _attackTriggers = GetComponentsInChildren<SwordAttackTrigger>().ToList();
            _attackTriggers.ForEach(attackTrigger => attackTrigger.Initialize());

            foreach (SwordAttackTrigger trigger in _attackTriggers)
            {
                trigger.OnTriggerEnter += HitTargetListener;
                trigger.OnTriggerDeactivation += TriggerDeactivationListener;

                InitSwordImpact(trigger.TargetAttackType);
            }
        }

        protected override void SetDefaultPosRelatedToOwner()
        {
            transform.position = Owner.Body.Transform.position;
        }

        private void InitSwordImpact(AttackType targetAttackType)
        {
            AttackImpact impact;

            if (targetAttackType == AttackType.Fast)
            {
                impact = BaseImpact;
            }
            else
            {
                impact = base.InitImpact(Settings.HeavyImpact);
            }
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

        public void TriggerAttack(AttackType attackType)
        {
            _currentAttackType = attackType;
            SwordAttackTrigger attackTrigger = _attackTriggers.FirstOrDefault(trigger => trigger.TargetAttackType == attackType);

            if (attackTrigger == null)
            {
                return;
            }

            attackTrigger.Activate(Settings.TriggerActivityTimeSec);
            AttackImpact impact = GetImpact(attackType);
            impact.Shake?.Activate();
        }

        protected override float RequestDamageAmount()
        {
            return _currentAttackType switch
            {
                AttackType.Fast => CalculateCurrentDamage(Settings.Damage),
                AttackType.Heavy => CalculateCurrentDamage(Settings.HeavyDamage),
                _ => throw new NotImplementedException()
            };
        }

        protected override AttackImpact GetActiveImpact()
        {
            // The base class will automatically call this and use the correct dictionary impact
            return _attackImpactDict.GetValueOrDefault(_currentAttackType, null);
        }

        private void TriggerDeactivationListener(IAttackTrigger trigger)
        {
            SwordAttackTrigger swordAttackTrigger = (SwordAttackTrigger)trigger;

            if (_attackImpactDict.TryGetValue(swordAttackTrigger.TargetAttackType, out AttackImpact impact))
            {
                impact.ReloadImpact();
            }
        }

        private AttackImpact GetImpact(AttackType attackType)
        {
            return _attackImpactDict.GetValueOrDefault(attackType, null);
        }

        #endregion

    }
}
