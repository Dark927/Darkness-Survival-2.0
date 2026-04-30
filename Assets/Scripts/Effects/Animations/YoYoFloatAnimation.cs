using UnityEngine;
using DG.Tweening;
using Utilities.UI;

namespace Visuals.Effects.Animations
{
    public class YoYoFloatAnimation : MonoBehaviour
    {
        #region Fields 

        [SerializeField] private float _floatOffset = 0.3f;
        [SerializeField] private float _duration = 1.5f;
        [SerializeField] private Ease _easeType = Ease.InOutSine;

        private Tween _floatTween;

        #endregion

        #region Methods

        private void Start()
        {
            bool isCreated = TweenHelper.TryCreateTween(
                gameObject,
                () => transform.DOMoveY(transform.position.y + _floatOffset, _duration)
                               .SetLoops(-1, LoopType.Yoyo)
                               .SetEase(_easeType),
                out _floatTween);

            if (isCreated)
            {
                _floatTween.Play();
            }
        }

        private void OnDestroy()
        {
            TweenHelper.KillTweenIfActive(_floatTween);
        }

        #endregion
    }
}
