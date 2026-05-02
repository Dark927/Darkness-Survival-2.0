using UnityEngine;
using DG.Tweening;
using Utilities.UI;

namespace Visuals.Effects.Animations
{
    public class YoYoScaleAnimation : MonoBehaviour
    {
        #region Fields 

        [SerializeField] private float _targetScaleMultiplier = 1.05f;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _easeType = Ease.InOutSine;

        private Tween _scaleTween;

        #endregion

        #region Methods

        private void Start()
        {
            // Calculate the target scale based on the object's initial size
            Vector3 targetScale = transform.localScale * _targetScaleMultiplier;

            bool isCreated = TweenHelper.TryCreateTween(
                gameObject,
                () => transform.DOScale(targetScale, _duration)
                               .SetLoops(-1, LoopType.Yoyo)
                               .SetEase(_easeType),
                out _scaleTween);

            if (isCreated)
            {
                _scaleTween.Play();
            }
        }

        private void OnDestroy()
        {
            TweenHelper.KillTweenIfActive(_scaleTween);
        }

        #endregion
    }
}
