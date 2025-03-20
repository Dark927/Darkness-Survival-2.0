using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.UI;

namespace UI
{
    public abstract class InteractiveAnimationsBaseUI : MonoBehaviour
    {
        private Sequence _currentAnimation;

        public Sequence CurrentAnimation => _currentAnimation;


        #region Pointer Listeners

        ////////
        // Listeners for Event Trigger configured with the Unity Inspector 
        ////////

        public virtual void PointerClickListener(BaseEventData data = default)
        {

        }

        public virtual void PointerEnterListener(BaseEventData data = default)
        {

        }

        public virtual void PointerExitListener(BaseEventData data = default)
        {

        }

        #endregion


        #region Current Animation Process 

        /// <summary>
        /// Replace current animation (kill previous one).
        /// </summary>
        /// <param name="targetAnimation">target animation</param>
        /// <param name="completeCurrentAnimation">false - just kill previous animation. true - complete previous animations (OnComplete event)</param>
        public void ReplaceCurrentAnimation(Sequence targetAnimation, bool completeCurrentAnimation = false)
        {
            StopActiveAnimation(completeCurrentAnimation);

            _currentAnimation = targetAnimation;
        }

        public void StopActiveAnimation(bool complete = false)
        {
            TweenHelper.KillTweenIfActive(_currentAnimation, complete);
        }

        public void DoDelayedActions(float timeDelay, Action callback)
        {
            DOVirtual.DelayedCall(timeDelay, () =>
            {
                callback?.Invoke();
            }).SetEase(Ease.Linear);
        }

        protected virtual void OnDestroy()
        {
            StopActiveAnimation();
        }

        #endregion
    }
}
