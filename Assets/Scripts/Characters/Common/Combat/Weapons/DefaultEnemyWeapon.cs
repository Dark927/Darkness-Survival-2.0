using Characters.Interfaces;
using Characters.Player;
using System;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class DefaultEnemyWeapon : CharacterWeaponBase
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

        private void Awake()
        {
            _attackTrigger = GetComponentInChildren<AttackTrigger>();
        }

        private void Start()
        {
            _attackTrigger.ScaleTriggerCollider(_triggerScaleMultiplier);
            _attackTrigger.OnTriggerStay += HitTargetListener;
            _attackTrigger.Activate();
        }

        public override void Dispose()
        {
            base.Dispose();
            _attackTrigger.OnTriggerStay -= HitTargetListener;
            _attackTrigger.Deactivate();
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
