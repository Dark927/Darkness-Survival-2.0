using Characters.Interfaces;
using Settings.CameraManagement.Shake;
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

        private ShakeImpact _fastAttackImpact;
        private ShakeImpact _heavyAttackImpact;
        private SwordAttackSettings _swordAttackSettings;

        #endregion


        #region Properties
        public SwordAttackSettings Settings => _swordAttackSettings;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            try
            {
                _swordAttackSettings = (SwordAttackSettings)AttackData.AttackSettings;
            }
            catch (Exception ex)
            {
                Debug.LogError($"# Can not convert the {(AttackData.AttackSettings.GetType())} to {nameof(SwordAttackSettings)}! Settings is NULL!");
                Debug.LogException(ex);
            }

            _attackTriggers = GetComponentsInChildren<SwordAttackTrigger>().ToList();
            _fastAttackImpact = new ShakeImpact(Settings.FastShakeSettings);
            _heavyAttackImpact = new ShakeImpact(Settings.HeavyShakeSettings);

            foreach (SwordAttackTrigger trigger in _attackTriggers)
            {
                trigger.OnTriggerEnter += HitTargetListener;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (SwordAttackTrigger trigger in _attackTriggers)
            {
                trigger.OnTriggerEnter -= HitTargetListener;
            }
        }

        #endregion

        public void TriggerAttack(AttackType attackType)
        {
            SwordAttackTrigger attackTrigger = _attackTriggers.FirstOrDefault(trigger => trigger.TargetAttackType == attackType);

            if (attackTrigger != null)
            {
                attackTrigger.Activate(Settings.TriggerActivityTimeSec);
            }

            ActivateImpact(attackType);
        }

        private void ActivateImpact(AttackType attackType)
        {
            switch (attackType)
            {
                case AttackType.Fast:
                    _fastAttackImpact.Activate();
                    break;


                case AttackType.Heavy:
                    _heavyAttackImpact.Activate();
                    break;


                default:
                    throw new NotImplementedException();
            }
        }

        protected override void HitTargetListener(object sender, EventArgs args)
        {
            SwordAttackTriggerArgs attackArgs = (SwordAttackTriggerArgs)args;
            GameObject targetObject = attackArgs.TargetCollider.gameObject;

            if (targetObject.TryGetComponent(out IDamageable target))
            {
                var damage = RequestDamage(attackArgs.AttackType);
                target.TakeDamage(damage);
            }
        }

        private float RequestDamage(AttackType attackType)
        {
            return attackType switch
            {
                AttackType.Fast => CalculateDefaultDamage(),
                AttackType.Heavy => CalculateHeavyDamage(),
                _ => throw new NotImplementedException()
            };
        }

        private float CalculateHeavyDamage()
        {
            return CalculateDamage(Settings.HeavyDamageSettings.Min, Settings.HeavyDamageSettings.Max);
        }

        #endregion

    }
}
