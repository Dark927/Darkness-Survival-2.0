using Characters.Common.Combat.Weapons.Data;
using Characters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class CharacterSword : WeaponBase
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

#if UNITY_EDITOR
            TempDebugCode();
#endif

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

        private void InitSwordImpact(AttackType targetAttackType)
        {
            AttackImpact impact = base.InitImpact(GetImpactSettings(targetAttackType));
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

        protected override float RequestDamage()
        {
            return _currentAttackType switch
            {
                AttackType.Fast => CalculateDamage(Settings.Damage),
                AttackType.Heavy => CalculateDamage(Settings.HeavyDamage),
                _ => throw new NotImplementedException()
            };
        }

        protected override void PerformImpact(Collider2D targetCollider)
        {
            if (!ImpactAvailable 
                || !_attackImpactDict.TryGetValue(_currentAttackType, out AttackImpact impact) 
                || !impact.IsReady
                || !impact.CanUseRandomly())
            {
                return;
            }

            IEntityPhysicsBody targetBody = targetCollider.GetComponent<IEntityPhysicsBody>();

            impact.AddKnockback(CalculatePushDirection(Center, targetCollider.bounds.center));
            impact.PerformPhysicsImpact(targetCollider);
        }

        protected void TriggerDeactivationListener(IAttackTrigger trigger)
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

        private ImpactSettings GetImpactSettings(AttackType type)
        {
            return type switch
            {
                AttackType.Fast => Settings.Impact,
                AttackType.Heavy => Settings.HeavyImpact,
                _ => throw new NotImplementedException(),
            };
        }


        // ToDo : remove this DEBUG code on Release.
        private void TempDebugCode()
        {
            try
            {
                _swordAttackSettings = (SwordAttackSettings)AttackSettings;
            }
            catch (Exception ex)
            {
                Debug.LogError($"# Can not convert the {(AttackSettings.GetType())} to {nameof(SwordAttackSettings)}! Settings is NULL!");
                Debug.LogException(ex);
            }
        }
        #endregion

    }
}
