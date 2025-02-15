using Characters.Common.Combat.Weapons.Data;
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

        private ShakeImpact _fastAttackImpactShake;
        private ShakeImpact _heavyAttackImpactShake;
        private SwordAttackSettings _swordAttackSettings;

        #endregion


        #region Properties
        public SwordAttackSettings Settings => _swordAttackSettings;

        #endregion


        #region Methods

        #region Init

        public override void Initialize(WeaponAttackData attackData)
        {
            base.Initialize(attackData);

#if UNITY_EDITOR
            try
            {
                _swordAttackSettings = (SwordAttackSettings)AttackData.AttackSettings;
            }
            catch (Exception ex)
            {
                Debug.LogError($"# Can not convert the {(AttackData.AttackSettings.GetType())} to {nameof(SwordAttackSettings)}! Settings is NULL!");
                Debug.LogException(ex);
            }
#endif

            _swordAttackSettings = (SwordAttackSettings)AttackData.AttackSettings;

            _attackTriggers = GetComponentsInChildren<SwordAttackTrigger>().ToList();
            _attackTriggers.ForEach(attackTrigger => attackTrigger.Initialize());

            _fastAttackImpactShake = new ShakeImpact(Settings.ImpactSettings.ShakeSettings);
            _heavyAttackImpactShake = new ShakeImpact(Settings.HeavyImpactSettings.ShakeSettings);

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

            GetImpact(attackType)?.Invoke();
        }

        private Action GetImpact(AttackType attackType)
        {
            return attackType switch
            {
                AttackType.Fast => Settings.ImpactSettings.UseImpact ? FastImpact : null,
                AttackType.Heavy => Settings.HeavyImpactSettings.UseImpact ? HeavyImpact : null,
                _ => null,
            };
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

        private void FastImpact()
        {
            _fastAttackImpactShake.Activate();
        }

        private void HeavyImpact()
        {
            _heavyAttackImpactShake.Activate();
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
