using Characters.Common.Combat.Weapons.Data;
using Characters.Interfaces;
using Characters.Player;
using System;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class DefaultEnemyWeapon : WeaponBase
    {
        #region Fields 

        [SerializeField] private float _triggerScaleMultiplier = 1f;
        private AttackTrigger _attackTrigger;

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
        }

        public override void Dispose()
        {
            base.Dispose();
            _attackTrigger.ScaleTriggerCollider(1f/_triggerScaleMultiplier);
            _attackTrigger.Deactivate();

            _attackTrigger.OnTriggerStay -= HitTargetListener;
        }

        protected override void HitTargetListener(object sender, EventArgs args)
        {
            AttackTriggerArgs attackArgs = (AttackTriggerArgs)args;
            GameObject targetObject = attackArgs.TargetCollider.gameObject;

            if (targetObject.TryGetComponent(out IDamageable target) && (target is PlayerCharacterBody))
            {
                float damage = CalculateDefaultDamage();
                target.TakeDamage(damage);
            }
        }

        #endregion
    }
}
