using System;
using UnityEngine;

namespace Characters.Player.Animation
{
    public class NeroAnimationEvents : MonoBehaviour
    {
        public event EventHandler AttackFinished;

        public void OnAttackFinished()
        {
            AttackFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}