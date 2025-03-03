using Characters.Common.Combat.Weapons.Data;
using Characters.Interfaces;
using Characters.Player;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class DefaultEnemyWeapon : WeaponBase
    {
        #region Fields 

        [SerializeField] private float _triggerScaleMultiplier = 1f;
        private AttackTrigger _attackTrigger;
        private AttackImpact _impact;

        #endregion


        #region Properties

        public AttackTrigger Trigger => _attackTrigger;
        public float TriggerScaleMultiplier => _triggerScaleMultiplier;

        #endregion


        #region Methods

        public override void Initialize(WeaponAttackDataBase attackData)
        {
            base.Initialize(attackData);
            _attackTrigger = GetComponentInChildren<AttackTrigger>();
            _attackTrigger.Initialize();

            _attackTrigger.ScaleTriggerCollider(_triggerScaleMultiplier);
            _attackTrigger.Activate();

            _attackTrigger.OnTriggerStay += HitTargetListener;

            _impact = InitImpact(attackData.Settings.Impact);
        }

        public override void Dispose()
        {
            base.Dispose();
            _attackTrigger.ScaleTriggerCollider(1f / _triggerScaleMultiplier);
            _attackTrigger.Deactivate();

            _attackTrigger.OnTriggerStay -= HitTargetListener;
        }

        protected override void PerformImpact(Collider2D targetCollider)
        {
            if (!ImpactAvailable
                || _impact == null
                || !_impact.IsReady
                || !_impact.CanUseRandomly())
            {
                return;
            }

            IEntityPhysicsBody targetBody = targetCollider.GetComponent<IEntityPhysicsBody>();

            _impact.AddKnockback(CalculatePushDirection(Center, targetBody.Physics.Collider.bounds.center));
            _impact.PerformPhysicsImpact(targetCollider);

            _impact.ReloadImpact();
        }

        protected override bool CheckHitTargetCondition(GameObject targetObject, out IDamageable target)
        {
            target = targetObject.GetComponent<IDamageable>();
            return (target != null) && (target is PlayerCharacterBody);
        }

        #endregion
    }
}
