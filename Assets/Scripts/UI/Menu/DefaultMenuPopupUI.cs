
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UI.Buttons;
using UnityEngine;
using Utilities.Attributes;
using Utilities.UI;

namespace UI
{
    public class DefaultMenuPopupUI : PopupUI
    {
        #region Fields 

        private List<ButtonContainerUI> _buttonsList;

        [Header(""), Space, CustomHeader("Menu Header", 3, 1, CustomHeaderAttribute.HeaderColor.cyan)]

        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private float _headerFadeDuration = 1f;
        [SerializeField] private float _headerScaleDuration = 1f;

        [Space, CustomHeader("Menu Buttons", 2, 1, CustomHeaderAttribute.HeaderColor.cyan)]

        [SerializeField] private float _buttonFadeDuration = 1f;
        [SerializeField] private float _buttonScaleDuration = 1f;

        private List<Sequence> _buttonsAnimationSequences;
        private Tween _titleAnimation;

        private Vector2 _titleStartScale;
        private Color _titleStartColor;

        private Vector2 _buttonStartScale;

        #endregion


        #region Methods 

        #region Init

        protected override void Awake()
        {
            base.Awake();
            _buttonsList = GetComponentsInChildren<ButtonContainerUI>().ToList();

            if (_headerText != null)
            {
                _titleStartScale = _headerText.transform.localScale;
                _titleStartColor = _headerText.color;
            }
        }

        public override void PrepareAnimation()
        {
            base.PrepareAnimation();
            ConfigureTitleStartParameters();
            ConfigureButtonStartParameters();
        }


        private void ConfigureTitleStartParameters()
        {
            if (_headerText == null)
            {
                return;
            }

            _headerText.transform.localScale = Vector2.zero;
            _headerText.color = Color.clear;
        }

        private void ConfigureButtonStartParameters()
        {
            if (_buttonsList.Count > 0)
            {
                _buttonStartScale = _buttonsList.FirstOrDefault().transform.localScale;
            }

            _buttonsList.ForEach(button =>
            {
                button.transform.localScale = Vector2.zero;
                button.CanvasGroup.alpha = 0f;
                button.CanvasGroup.interactable = false;
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            KillActiveButtonsAnimation();
            TweenHelper.KillTweenIfActive(_titleAnimation);
        }

        #endregion

        public override void Show(Action callback = null)
        {
            base.Show(() =>
            {
                _buttonsAnimationSequences = PlayButtonsAnimation(1f, _buttonStartScale);
                _titleAnimation = PlayTitleAnimation(_titleStartColor, _titleStartScale);
                callback?.Invoke();
            });
        }


        private Sequence PlayTitleAnimation(Color targetColor, Vector2 targetScale)
        {
            if (_headerText == null)
            {
                return null;
            }

            Sequence animation = DOTween.Sequence();

            animation
                .Append(
                    _headerText
                        .DOColor(targetColor, _headerFadeDuration)
                        .From(_headerText.color)
                )
                .Join(
                    _headerText.transform
                        .DOScale(targetScale, _headerScaleDuration)
                        .From(_headerText.transform.localScale)
                );

            animation
                .SetUpdate(true);

            return animation;
        }

        private List<Sequence> PlayButtonsAnimation(float targetAlpha, Vector2 targetScale)
        {
            List<Sequence> animationSequences = new List<Sequence>();

            _buttonsList?.ForEach(buttonContainer =>
            {
                animationSequences.Add(PlaySingleButtonAnimation(buttonContainer, targetAlpha, targetScale));
            });

            return animationSequences;
        }

        private Sequence PlaySingleButtonAnimation(ButtonContainerUI buttonContainer, float targetAlpha, Vector2 targetScale)
        {
            Sequence animationSequence = DOTween.Sequence();
            RectTransform buttonTransform = buttonContainer.GetComponent<RectTransform>();

            animationSequence
                .Append(
                    buttonContainer.CanvasGroup
                        .DOFade(targetAlpha, _buttonFadeDuration)
                        .From(buttonContainer.CanvasGroup.alpha))
                .Join(
                    buttonTransform.DOScale(targetScale, _buttonScaleDuration)
                        .From(buttonContainer.transform.localScale)
                        .OnComplete(() => buttonContainer.CanvasGroup.interactable = true)
                );

            animationSequence
                .SetUpdate(true);

            return animationSequence;
        }

        private void KillActiveButtonsAnimation()
        {
            _buttonsAnimationSequences?.ForEach(animation =>
            {
                TweenHelper.KillTweenIfActive(animation, true);
            });

            _buttonsAnimationSequences?.Clear();
        }

        #endregion
    }
}
