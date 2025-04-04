using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utilities.UI;

namespace UI
{
    [System.Serializable]
    public class ElementColorFadeUI<TTargetElement> : IDisposable where TTargetElement : Graphic
    {
        #region Fields 

        [Header("Fade transition - Settings")]
        [SerializeField] private float _transitionDuration = 1f;
        [SerializeField] private AnimationCurve _fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private TTargetElement _target;
        private Tween _animation;

        #endregion


        #region Methods

        public ElementColorFadeUI(TTargetElement target)
        {
            SetTarget(target);
        }

        public void Dispose()
        {
            StopActiveAnimation();
        }


        public void SetTarget(TTargetElement target)
        {
            _target = target;
        }

        public void Fade(Color fadeColor, float speedMult = 1f)
        {
            StopActiveAnimation();

            _animation = PlayFadeAnimation(_target, fadeColor);
            _animation.timeScale = speedMult;
        }

        private void StopActiveAnimation()
        {
            TweenHelper.KillTweenIfActive(_animation);
        }

        private Tween PlayFadeAnimation(TTargetElement targetElement, Color targetColor)
        {
            return targetElement
                    .DOColor(targetColor, _transitionDuration)
                    .SetEase(_fadeCurve)
                    .SetUpdate(true);
        }

        #endregion
    }
}
