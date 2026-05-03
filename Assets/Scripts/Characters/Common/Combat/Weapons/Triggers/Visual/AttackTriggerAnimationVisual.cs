using UnityEngine;
using Utilities.ErrorHandling;

namespace Characters.Common.Combat
{
    public class AttackTriggerAnimationVisual : AttackTriggerVisual
    {
        [Header("Animation Settings")]
        [SerializeField] private Animator _animator;

        // calculated automatically
        private float _originalClipLengthSec = 1f;

        protected override void Awake()
        {
            base.Awake();

            // Automatically extract the length of the first animation clip
            if (_animator != null && _animator.runtimeAnimatorController != null)
            {
                AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;

                if (clips.Length > 0)
                {
                    _originalClipLengthSec = clips[0].length;
                }
                else
                {
                    ErrorLogger.LogWarning($"[{gameObject.name}] Animator Controller has no animation clips! Defaulting to 1 sec.");
                }
            }
        }

        protected override void ShowVisual(IAttackTrigger trigger)
        {
            base.ShowVisual(trigger);

            if (_animator != null)
            {
                if (trigger.CurrentActivationTime > 0)
                {
                    // Calculate the exact speed multiplier needed
                    float targetSpeed = _originalClipLengthSec / trigger.CurrentActivationTime;
                    _animator.speed = targetSpeed;
                }

                _animator.Play(0, -1, 0f);
            }
        }
    }
}
