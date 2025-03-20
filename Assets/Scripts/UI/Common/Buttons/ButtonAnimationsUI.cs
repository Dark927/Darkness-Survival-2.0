using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UI.CustomCursor;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.Animations;


namespace UI.Buttons
{
    public class ButtonAnimationsUI : InteractiveAnimationsBaseUI
    {
        #region Fields 

        [SerializeField] private ButtonAnimationDataUI _animationData;

        private TextMeshProUGUI _buttonText;
        private List<ButtonBorderUI> _buttonBorders;

        private ButtonAnimationParamsUI _startSettingsParams;
        private ButtonAnimationParamsUI _targetSettingsParams;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
            _buttonBorders = GetComponentsInChildren<ButtonBorderUI>().ToList();

            _startSettingsParams = new ButtonAnimationParamsUI()
            {
                TargetScale = transform.localScale
            };
        }

        private void Start()
        {
            _startSettingsParams.DurationInSec = _animationData.HoverAnimationParams.DurationInSec;
            _startSettingsParams.TargetTitleColor = _buttonText.color;
            _startSettingsParams.TargetBordersColor = _buttonBorders.Count > 0 ? _buttonBorders.First().Img.color : Color.white;
            _startSettingsParams.TargetTitleSize = _buttonText.fontSize;
        }

        #endregion

        ////////
        // Listeners for Event Trigger configured with the Unity Inspector 
        ////////

#pragma warning disable IDE0060 // Remove unused parameter

        public override void PointerEnterListener(BaseEventData data = default)
        {
            MouseCursor.Instance.HoverInteractiveUI();

            _targetSettingsParams = _animationData.HoverAnimationParams;
            _targetSettingsParams.TargetTitleSize = _startSettingsParams.TargetTitleSize * _animationData.HoverAnimationParams.TargetScale.x;

            ReplaceCurrentAnimation(PlayAnimation(_targetSettingsParams), false);
        }

        public override void PointerExitListener(BaseEventData data = default)
        {
            MouseCursor.Instance.ExitInteractiveUI();

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            ReplaceCurrentAnimation(PlayAnimation(_startSettingsParams), false);
        }

        public override void PointerClickListener(BaseEventData data = default)
        {
            _targetSettingsParams = _animationData.ClickAnimationParams;
            _targetSettingsParams.TargetTitleSize = _buttonText.fontSize * _animationData.ClickAnimationParams.TargetScale.x;
            _targetSettingsParams.TargetScale = transform.localScale * _animationData.ClickAnimationParams.TargetScale;

            ReplaceCurrentAnimation(PlayAnimation(_targetSettingsParams), true);
            CurrentAnimation.SetLoops(2, LoopType.Yoyo);
        }

#pragma warning restore IDE0060 // Remove unused parameter

        ////////


        private Sequence PlayAnimation(ButtonAnimationParamsUI parameters)
        {
            Sequence animation = DOTween.Sequence();

            animation
                .Append(
                    _buttonText
                        .DOColor(parameters.TargetTitleColor, parameters.DurationInSec)
                        .From(_buttonText.color)
                )
                .Join(
                    _buttonText
                        .DOScale(parameters.TargetTitleSize, parameters.DurationInSec)
                        .From(_buttonText.fontSize)
                )
                .Join(
                    transform
                        .DOScale(parameters.TargetScale, parameters.DurationInSec)
                        .From(transform.localScale)
                );

            _buttonBorders.ForEach(border =>
                animation.Join(
                    border.Img
                        .DOColor(parameters.TargetBordersColor, parameters.DurationInSec)
                        .From(border.Img.color))
            );

            animation.SetUpdate(true);
            return animation;
        }

        #endregion
    }
}
