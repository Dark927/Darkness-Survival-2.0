using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;
using Utilities.UI;

namespace UI
{
    public class PopupUI : InteractiveAnimationsBaseUI, IPopupUI
    {
        #region Fields 

        [CustomHeader("Popup Main", 1, 0, CustomHeaderAttribute.HeaderColor.green)]

        [SerializeField] private CanvasGroup _bodyAlphaGroup;

        [Space, CustomHeader("Common animation", 4, 1, CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private bool _useShift = false;
        [SerializeField] private float _shiftPosDuration = 1f;
        [SerializeField] private float _scaleDuration = 0.5f;

        [CustomHeader("Deactivation settings", 2, 2, CustomHeaderAttribute.HeaderColor.yellow)]
        [SerializeField] private float _deactivationSpeedMult = 1f;


        [Header(""), Space, CustomHeader("Menu Background", 3, 1, CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private Graphic _background;
        [SerializeField] private float _backgroundFadeDuration;

        private float _backgroundStartAlpha;

        private RectTransform _bodyTransform;
        private Vector2 _targetBodyPosition;
        private Vector2 _targetBodyScale;
        private Vector2 _startShift;

        private Tween _backgroundAnimation;

        #endregion


        #region Properties 

        public float DeactivationSpeedMult => _deactivationSpeedMult;
        public float ScaleDuration => _scaleDuration;

        #endregion


        #region Methods

        #region Init

        protected virtual void Awake()
        {
            _bodyTransform = _bodyAlphaGroup.GetComponent<RectTransform>();

            _targetBodyPosition = _bodyTransform.anchoredPosition;
            _targetBodyScale = _bodyTransform.localScale;
            _startShift = new Vector2(_targetBodyPosition.x, -Screen.height / 2f);

            if (_background != null)
            {
                _backgroundStartAlpha = _background.color.a;
            }
        }

        public virtual void PrepareAnimation()
        {
            ConfigureTransformStartParameters();
            ConfigureAlphaGroupStartParameters();
            ConfigureBackgroundStartParameters();
        }

        private void ConfigureTransformStartParameters()
        {
            _bodyTransform.localScale = Vector2.zero;

            if (_useShift)
            {
                _bodyTransform.anchoredPosition = _startShift;
            }
        }

        private void ConfigureAlphaGroupStartParameters()
        {
            _bodyAlphaGroup.alpha = 0f;
            _bodyAlphaGroup.interactable = false;
        }

        private void ConfigureBackgroundStartParameters()
        {
            if (_background == null)
            {
                return;
            }

            Color zeroAlphaBackground = _background.color;
            zeroAlphaBackground.a = 0f;
            _background.color = zeroAlphaBackground;
        }


        #endregion


        public virtual void Show(Action callback = null)
        {
            // Main animations

            ReplaceCurrentAnimation(GetMainElementsAnimation(_bodyAlphaGroup, 1f, _bodyTransform, _targetBodyScale, _targetBodyPosition), false);

            // Extra animations

            CurrentAnimation.Join(GetExtraElementAnimation(_background, _backgroundStartAlpha, _backgroundFadeDuration));

            CurrentAnimation
                .OnComplete(() =>
                {
                    _bodyAlphaGroup.interactable = true;
                    callback?.Invoke();
                });

            CurrentAnimation
                .SetEase(Ease.InOutSine)
                .SetUpdate(true)
                .Play();
        }

        public virtual void Hide(Action callback = null)
        {
            _bodyAlphaGroup.interactable = false;

            // Main animations

            ReplaceCurrentAnimation(GetMainElementsAnimation(_bodyAlphaGroup, 0f, _bodyTransform, Vector2.zero, _startShift), false);

            // Extra animations

            CurrentAnimation.Join(GetExtraElementAnimation(_background, 0f, _backgroundFadeDuration));

            CurrentAnimation.timeScale = _deactivationSpeedMult;

            CurrentAnimation
                .OnComplete(() =>
                {
                    callback?.Invoke();
                });

            CurrentAnimation
                .SetEase(Ease.InOutSine)
                .SetUpdate(true);

            CurrentAnimation.Play();
        }

        private Tween GetExtraElementAnimation(Graphic targetImage, float targetAlpha, float fadeDuration)
        {
            if (TweenHelper.TryCreateTween(targetImage, () =>
            {
                return targetImage
                            .DOFade(targetAlpha, fadeDuration)
                            .From(targetImage.color.a)
                            .SetEase(Ease.InOutCubic);
            }, out Tween createdFadeTween))
            {
                return createdFadeTween;
            }

            return DOTween.Sequence();
        }

        private Sequence GetMainElementsAnimation(
            CanvasGroup alphaGroup, float targetAlpha,
            RectTransform targetTransform, Vector2 targetScale, Vector2 targetShift = default)
        {
            Sequence animation = DOTween.Sequence();

            animation
                .Append(
                    alphaGroup
                            .DOFade(targetAlpha, _fadeDuration)
                            .From(alphaGroup.alpha)
            )
                .Join(

                    targetTransform
                            .DOScale(targetScale, _scaleDuration)
                            .From(targetTransform.localScale)
            );

            if (_useShift)
            {
                animation
                    .Join(
                        targetTransform
                                .DOAnchorPos(targetShift, _shiftPosDuration)
                                .From(targetTransform.anchoredPosition));
            }

            return animation.Pause();
        }

        #endregion
    }
}
