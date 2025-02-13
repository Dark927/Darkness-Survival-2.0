using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class BasicAttack : IDisposable
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

        private readonly CharacterBodyBase _characterBody;
        private List<CharacterWeaponBase> _basicWeapons;

        #endregion


        #region Properties

        protected CharacterBodyBase CharacterBody => _characterBody;
        public List<CharacterWeaponBase> BasicWeapons => _basicWeapons;

        #endregion


        #region Methods 

        #region Init 

        public BasicAttack(CharacterBodyBase characterBody, List<CharacterWeaponBase> basicWeapons)
        {
            _characterBody = characterBody;
            _basicWeapons = basicWeapons;
        }

        public virtual void Init()
        {

        }

        public virtual void Dispose()
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
        }

        protected void FinishAttack()
        {
            OnAttackFinished?.Invoke();
            _isAttacking = false;
        }

        #endregion
    }
}
