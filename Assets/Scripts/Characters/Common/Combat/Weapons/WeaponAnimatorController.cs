using System;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// Attach this to the GameObject containing the Animator.
    /// Acts as a highly reusable bridge between the Weapon logic and the Unity Animator.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class WeaponAnimatorController : MonoBehaviour
    {
        [Header("Animator Parameters")]
        [Tooltip("The name of the float parameter used to control animation speed.")]
        [SerializeField] private string _attackSpeedParam = "AttackSpeed";

        [Tooltip("The name of the trigger parameter used to fire an attack.")]
        [SerializeField] private string _attackTriggerParam = "Pulse";

        private Animator _animator;

        // Hashes are cached for maximum performance
        private int _attackSpeedHash;
        private int _attackTriggerHash;

        // The event a weapon will subscribe to
        public event Action OnAttackImpactTriggered;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _attackSpeedHash = Animator.StringToHash(_attackSpeedParam);
            _attackTriggerHash = Animator.StringToHash(_attackTriggerParam);
        }

        public void SetAttackSpeedMultiplier(float speedMultiplier)
        {
            if (_animator != null)
            {
                _animator.SetFloat(_attackSpeedHash, speedMultiplier);
            }
        }

        public void TriggerAttackAnimation()
        {
            if (_animator != null)
            {
                _animator.SetTrigger(_attackTriggerHash);
            }
        }

        /// <summary>
        /// ANIMATION EVENT TARGET: 
        /// For usage : Type 'TriggerAttackImpact' exactly like this into Unity Animation Event window
        /// </summary>
        public void TriggerAttackImpact()
        {
            OnAttackImpactTriggered?.Invoke();
        }
    }
}
