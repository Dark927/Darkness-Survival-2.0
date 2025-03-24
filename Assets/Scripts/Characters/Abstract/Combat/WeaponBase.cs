
using System;
using Characters.Common.Combat.Weapons.Data;
using Characters.Interfaces;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public abstract class WeaponBase : MonoBehaviour, IWeapon
    {
        #region Fields

        private Collider2D _ownerCollidder;
        private float _damageMultiplier = 1f;
        private ImpactSettings _impactSettings;
        private IAttackSettings _attackSettings;

        private Damage _calculatedDamage;

        #endregion

        #region Properties

        public IAttackSettings AttackSettings => _attackSettings;
        public bool ImpactAvailable => AttackSettings.Impact.UseImpact;
        public Vector3 Center => _ownerCollidder.bounds.center;
        public float DamageMultiplier => _damageMultiplier;
        public GameObject GameObject => this.gameObject;

        #endregion


        #region Methods

        #region Init

        public virtual void Initialize(WeaponAttackDataBase attackData)
        {
            IEntityPhysicsBody ownerBody = GetComponentInParent<IEntityPhysicsBody>(true);
            _ownerCollidder = ownerBody.Physics.Collider;
            _attackSettings = attackData.Settings;
            _calculatedDamage.NegativeStatus = attackData.Settings.NegativeStatus.Settings;
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

        public static float CalculateDamage(DamageSettings damageSettings, float damageMultiplier = 1f)
        {
            return UnityEngine.Random.Range(damageSettings.Min, damageSettings.Max) * damageMultiplier;
        }

        public static Vector2 CalculatePushDirection(Vector2 source, Vector2 target)
        {
            return (target - source).normalized;
        }


        public void SetCharacterDamageMultiplier(float damageMultiplier)
        {
            _damageMultiplier = damageMultiplier;
        }

        protected virtual void HitTargetListener(object sender, EventArgs args)
        {
            AttackTriggerArgs attackArgs = (AttackTriggerArgs)args;
            GameObject targetObject = attackArgs.TargetCollider.gameObject;

            if (!CheckHitTargetCondition(targetObject, out IDamageable target))
            {
                return;
            }

            _calculatedDamage.Amount = RequestDamageAmount();
            IEntityPhysicsBody targetBody = (target as IEntityPhysicsBody);

            if (target.CanAcceptDamage)
            {
                target.TakeDamage(_calculatedDamage);
                PerformPostDamageActions(attackArgs.TargetCollider);
            }
        }

        protected virtual float RequestDamageAmount()
        {
            return CalculateDamage(AttackSettings.Damage, _damageMultiplier);
        }

        protected virtual void PerformPostDamageActions(Collider2D targetCollider)
        {
            IEntityPhysicsBody targetBody = targetCollider.GetComponent<IEntityPhysicsBody>();

            if (!targetBody.Physics.IsImmune)
            {
                PerformImpact(targetCollider);
            }
            FlashTarget(targetCollider, _calculatedDamage.NegativeStatus);
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


        // ToDo : implement full ver.
        public void ApplyNewDamagePercent(float damagePercent)
        {
            Debug.Log("damage before -> " + _attackSettings.Damage.Min + "  -  " + _attackSettings.Damage.Max);
            _attackSettings.Damage = _attackSettings.Damage * damagePercent;
            Debug.Log("damage after -> " + _attackSettings.Damage.Min + "  -  " + _attackSettings.Damage.Max);
        }

        #endregion
    }
}
