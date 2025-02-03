using Characters.Animation;
using System;
using UnityEngine;

namespace Characters.Player.Animation
{
    public class CharacterAnimationEvents : MonoBehaviour, IAnimationEvents
    {
        public event Action OnAttackFinished;
        public event Action OnDeathFinished;

        private void FinishAttack()
        {
            OnAttackFinished?.Invoke();
        }

        private void FinishDeath()
        {
            OnDeathFinished?.Invoke();
        }
    }
}