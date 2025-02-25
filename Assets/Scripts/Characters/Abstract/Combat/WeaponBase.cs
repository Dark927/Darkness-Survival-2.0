﻿
using Characters.Common.Combat.Weapons.Data;
using Characters.Common.Physics2D;
using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public abstract class WeaponBase : MonoBehaviour, IWeapon, IDisposable
    {
        #region Fields

        private WeaponAttackDataBase _weaponAttackData;
        private Damage _damage;
        private Collider2D _ownerCollidder;

        #endregion

        #region Properties

        public WeaponAttackDataBase AttackCommonData => _weaponAttackData;
        public IAttackSettings AttackSettings => _weaponAttackData.Settings;
        public bool ImpactAvailable => AttackSettings.Impact.UseImpact;
        public Vector3 Center => _ownerCollidder.bounds.center;


        #endregion


        #region Methods

        #region Init

        public virtual void Initialize(WeaponAttackDataBase attackData)
        {
            IEntityPhysicsBody ownerBody = GetComponentInParent<IEntityPhysicsBody>(true);
            _ownerCollidder = ownerBody.Physics.Collider;
            _weaponAttackData = attackData;
            _damage.NegativeStatus = attackData.Settings.NegativeStatus.Settings;
        }

        protected virtual AttackImpact InitImpact(ImpactSettings impactSettings)
        {
            AttackImpact impact = new AttackImpact(impactSettings);
            impact.AddStun();
            return impact;
        }

        public virtual void Dispose()
        {

        }

        #endregion

        public static float CalculateDamage(DamageSettings damageSettings)
        {
            return UnityEngine.Random.Range(damageSettings.Min, damageSettings.Max);
        }

        public static Vector2 CalculatePushDirection(Vector2 source, Vector2 target)
        {
            return (target - source).normalized;
        }


        protected virtual void HitTargetListener(object sender, EventArgs args)
        {
            AttackTriggerArgs attackArgs = (AttackTriggerArgs)args;
            GameObject targetObject = attackArgs.TargetCollider.gameObject;

            if (!CheckHitTargetCondition(targetObject, out IDamageable target))
            {
                return;
            }

            _damage.Amount = RequestDamageAmount();
            IEntityPhysicsBody targetBody = (target as IEntityPhysicsBody);

            if (target.CanAcceptDamage)
            {
                target.TakeDamage(_damage);
                PerformPostDamageActions(attackArgs.TargetCollider);
            }
        }

        protected virtual float RequestDamageAmount()
        {
            return CalculateDamage(AttackSettings.Damage);
        }

        protected virtual void PerformPostDamageActions(Collider2D targetCollider)
        {
            IEntityPhysicsBody targetBody = targetCollider.GetComponent<IEntityPhysicsBody>();

            if (!targetBody.Physics.IsImmune)
            {
                PerformImpact(targetCollider);
            }
            FlashTarget(targetCollider, _damage.NegativeStatus);
        }

        protected virtual void FlashTarget(Collider2D targetCollider, AttackNegativeStatus negativeStatus)
        {
            if (negativeStatus.Equals(AttackNegativeStatus.Zero))
            {
                return;
            }

            if (targetCollider.TryGetComponent(out IEntityPhysicsBody targetBody))
            {
                targetBody.Visual.ActivateColorBlink(
                    negativeStatus.VisualColor,
                    negativeStatus.EffectDurationInSec,
                    negativeStatus.EffectSpeedInSec);
            }
        }

        protected virtual void PerformImpact(Collider2D targetCollider)
        {

        }

        protected virtual bool CheckHitTargetCondition(GameObject targetObject, out IDamageable target)
        {
            target = targetObject.GetComponent<IDamageable>();
            return (target != null);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion
    }
}
