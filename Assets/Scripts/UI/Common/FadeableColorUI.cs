using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Math;
using Utilities.UI;

namespace UI
{
    public class FadeableColorUI<TTargetElement> : MonoBehaviour where TTargetElement : Graphic
    {
        #region Fields 

        [Header("Fade transition - Settings")]
        [SerializeField] private float _transitionDuration = 1f;
        [SerializeField] private AnimationCurve _fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private Tween _animation;

        #endregion


        #region Methods

        public void Fade(TTargetElement targetElement, Color fadeColor, float speedMult = 1f)
        {
            StopActiveAnimation();

            _animation = PlayFadeAnimation(targetElement, fadeColor);
            _animation.timeScale = speedMult;
        }

        protected void StopActiveAnimation()
        {
            TweenHelper.KillTweenIfActive(_animation);
        }

        protected virtual void OnDestroy()
        {
            StopActiveAnimation();
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
