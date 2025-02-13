using Characters.Interfaces;
using Settings.Global;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class BasicAttack : IEventListener
    {
        public enum Type
        {
            Default = 0,
            Fast,
            Heavy
        }

        #region Fields 

        public event Action<Type> OnAttackOfTypeStarted;
        public event Action OnAnyAttackStarted;
        public event Action OnAttackFinished;

        private bool _isAttacking = false;

        private readonly IEntityBody _entityBody;
        private List<WeaponBase> _basicWeapons;

        #endregion


        #region Properties

        protected IEntityBody EntityBody => _entityBody;
        public List<WeaponBase> BasicWeapons => _basicWeapons;

        #endregion


        #region Methods 

        #region Init 

        public BasicAttack(IEntityBody characterBody, List<WeaponBase> basicWeapons)
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

        protected void TryPerformAttack(Type type)
        {
            if (_isAttacking || (BasicWeapons.Count == 0))
            {
                return;
            }

            PerformAttack(type);
        }

        protected void PerformAttack(Type type)
        {
            _isAttacking = true;

            switch (type)
            {
                case Type.Default:
                case Type.Fast:
                    OnAttackOfTypeStarted?.Invoke(Type.Fast);
                    break;

                case Type.Heavy:
                    OnAttackOfTypeStarted?.Invoke(Type.Heavy);
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
