
using Characters.Common.Combat.Weapons.Data;
using Characters.Interfaces;
using System;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public abstract class WeaponBase : MonoBehaviour, IWeapon, IDisposable
    {
        private WeaponAttackData _weaponAttackData;
        public WeaponAttackData AttackData => _weaponAttackData;


        #region Methods

        #region Init

        public virtual void Initialize(WeaponAttackData attackData)
        {
            _weaponAttackData = attackData;
        }

        public virtual void Dispose()
        {

        }

        #endregion

        public static float CalculateDamage(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        protected float CalculateDefaultDamage()
        {
            return CalculateDamage(AttackData.AttackSettings.Damage.Min, AttackData.AttackSettings.Damage.Max);
        }

        protected virtual void HitTargetListener(object sender, EventArgs args)
        {
            AttackTriggerArgs attackArgs = (AttackTriggerArgs)args;
            GameObject targetObject = attackArgs.TargetCollider.gameObject;

            if (targetObject.TryGetComponent(out IDamageable target))
            {
                float damage = CalculateDefaultDamage();
                target.TakeDamage(damage);
                Debug.Log(targetObject.name);
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion
    }
}
