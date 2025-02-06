
using Characters.Common.Combat.Weapons;
using Characters.Player.Animation;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.Attacks
{
    public class CharacterBasicAttack : IDisposable
    {
        public enum Type
        {
            Reset = 0,
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
        private CharacterAnimatorController _animatorController;

        #endregion


        #region Properties

        protected CharacterAnimatorController AnimatorController => _animatorController;
        public List<CharacterWeaponBase> BasicWeapons => _basicWeapons;

        #endregion


        #region Methods 

        #region Init 

        public CharacterBasicAttack(CharacterBodyBase characterBody, List<CharacterWeaponBase> basicWeapons)
        {
            _characterBody = characterBody;
            _basicWeapons = basicWeapons;
        }

        public virtual void Init()
        {
            _animatorController = _characterBody.Visual.GetAnimatorController<CharacterAnimatorController>();
            OnAttackOfTypeStarted += _animatorController.TriggerAttack;
            AnimatorController.Events.OnAttackFinished += FinishAttack;
        }

        public virtual void Dispose()
        {
            OnAttackOfTypeStarted -= _animatorController.TriggerAttack;
            AnimatorController.Events.OnAttackFinished -= FinishAttack;
        }

        #endregion

        public void AttackPerformedListener(InputAction.CallbackContext context)
        {
            int contextValue = (int)context.ReadValue<float>();
            TryPerformAttack((Type)contextValue);

        }

        private void TryPerformAttack(Type type)
        {
            if (_isAttacking || (BasicWeapons.Count == 0))
            {
                return;
            }

            PerformAttack(type);
        }

        private void PerformAttack(Type type)
        {
            _isAttacking = true;

            switch (type)
            {
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

        public void FinishAttack()
        {
            OnAttackFinished?.Invoke();
            _isAttacking = false;
        }
    }

    #endregion
}