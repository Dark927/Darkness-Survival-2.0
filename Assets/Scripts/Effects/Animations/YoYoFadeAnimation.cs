using UnityEngine;
using DG.Tweening;
using Utilities.UI;

namespace Visuals.Effects.Animations
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class YoYoFadeAnimation : MonoBehaviour
    {
        #region Fields 

        [SerializeField] private float _targetAlpha = 0.4f;
        [SerializeField] private float _duration = 1.5f;
        [SerializeField] private Ease _easeType = Ease.InOutSine;

        private SpriteRenderer _spriteRenderer;
        private Tween _fadeTween;

        #endregion

        #region Methods

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            // Pass the SpriteRenderer as the safety check target
            bool isCreated = TweenHelper.TryCreateTween(
                _spriteRenderer,
                () => _spriteRenderer.DOFade(_targetAlpha, _duration)
                                     .SetLoops(-1, LoopType.Yoyo)
                                     .SetEase(_easeType),
                out _fadeTween);

            if (isCreated)
            {
                _fadeTween.Play();
            }
        }

        private void OnDestroy()
        {
            TweenHelper.KillTweenIfActive(_fadeTween);
        }

        #endregion
    }
}
