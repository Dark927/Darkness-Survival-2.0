using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Interfaces;
using Settings.Global;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class BasicAttack : IEventsConfigurable
    {
        public enum LocalType
        {
            Default = 0,
            Fast,
            Heavy
        }

        #region Fields 

        public event Action<BasicAttack.LocalType> OnAttackOfTypeStarted;
        public event Action OnAnyAttackStarted;
        public event Action OnAttackFinished;

        private bool _isAttacking = false;

        private readonly IEntityPhysicsBody _entityBody;
        private IEnumerable<IWeapon> _basicWeapons;

        #endregion


        #region Properties

        protected IEntityPhysicsBody EntityBody => _entityBody;
        protected virtual bool CanAttack => !_isAttacking && !(BasicWeapons.Count() == 0);
        public IEnumerable<IWeapon> BasicWeapons => _basicWeapons;

        #endregion


        #region Methods 

        #region Init 

        public BasicAttack(IEntityPhysicsBody characterBody, IEnumerable<IWeapon> basicWeapons)
        {
            _entityBody = characterBody;
            _basicWeapons = basicWeapons;
        }

        public virtual void Init()
        {

        }

        public virtual void ConfigureEventLinks()
        {

        }

        public virtual void RemoveEventLinks()
        {

        }

        #endregion

        public void TryPerformAttack(LocalType type)
        {
            if (CanAttack)
            {
                PerformAttack(type);
            }

        }

        protected void PerformAttack(LocalType type)
        {
            _isAttacking = true;

            switch (type)
            {
                case LocalType.Default:
                case LocalType.Fast:
                    OnAttackOfTypeStarted?.Invoke(LocalType.Fast);
                    break;

                case LocalType.Heavy:
                    OnAttackOfTypeStarted?.Invoke(LocalType.Heavy);
                    break;

                default:
                    Debug.LogWarning(" # Not implemented type of attack");
                    break;
            }
            OnAnyAttackStarted?.Invoke();
            AnyAttackStarted();
        }

        protected void RaiseAttackFinished()
        {
            OnAttackFinished?.Invoke();
            _isAttacking = false;
            AttackFinished();
        }

        protected virtual void AnyAttackStarted()
        {

        }

        protected virtual void AttackFinished()
        {

        }

        #endregion
    }
}
