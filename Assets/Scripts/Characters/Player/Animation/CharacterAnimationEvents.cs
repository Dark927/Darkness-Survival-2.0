using Characters.Animation;
using Characters.Player.Weapons;
using System;
using UnityEngine;

namespace Characters.Player.Animation
{
    public class CharacterAnimationEvents : MonoBehaviour, IAnimationEvents
    {
        #region Events 

        public event Action OnAttackFinished;
        public event Action<CharacterBasicAttack.Type> OnAttackHit;

        public event Action OnDeathFinished;

        #endregion


        #region Animation Methods

        #region Attacks 

        private void FastAttackHit()
        {
            OnAttackHit?.Invoke(CharacterBasicAttack.Type.Fast);
        }

        private void HeavyAttackHit()
        {
            OnAttackHit?.Invoke(CharacterBasicAttack.Type.Heavy);
        }

        private void FinishAttack()
        {
            OnAttackFinished?.Invoke();
        }

        #endregion


        private void FinishDeath()
        {
            OnDeathFinished?.Invoke();
        }

        #endregion
    }
}