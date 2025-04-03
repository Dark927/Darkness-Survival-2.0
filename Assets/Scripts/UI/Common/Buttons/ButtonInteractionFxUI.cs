using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Settings.Global;
using Settings.Global.Audio;
using TMPro;
using UI.Audio;
using UI.CustomCursor;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.Animations;
using Utilities.Attributes;


namespace UI.Buttons
{
    public class ButtonInteractionFxUI : InteractiveAnimationsBaseUI
    {
        #region Fields 

        private SoundsPlayer _soundsPlayer;

        [CustomHeader("Animations")]
        [SerializeField] private ButtonAnimationDataUI _animationData;

        [CustomHeader("Audio")]
        [SerializeField] private AudioEffectsDataUI _audioEffectsData;

        [Header("<color=yellow>Optional overrides</color>")]
        [SerializeField] private AudioEffectsSettingsUI _audioEffectsSettings;

        private TextMeshProUGUI _buttonText;
        private List<ButtonBorderUI> _buttonBorders;
        private float _lastClickTime;

        private ButtonAnimationParamsUI _startSettingsParams;
        private ButtonAnimationParamsUI _targetSettingsParams;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            if (_audioEffectsData != null)
            {
                _audioEffectsSettings = AudioEffectsSettingsUI.FillEmpty(_audioEffectsSettings, _audioEffectsData.Effects);
            }

            _lastClickTime = 0f;
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

            var audioService = ServiceLocator.Current.Get<GameAudioService>();
            _soundsPlayer = audioService.SoundsPlayer;
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

            CurrentAnimation.InsertCallback(_audioEffectsSettings.PlayDelay, () =>
            {
                _soundsPlayer.Play2DSound(_audioEffectsSettings.HoverSFX);
            });
        }

        public override void PointerExitListener(BaseEventData data = default)
        {
            MouseCursor.Instance.ExitInteractiveUI();

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            ReplaceCurrentAnimation(PlayAnimation(_startSettingsParams), false);

            CurrentAnimation.InsertCallback(_audioEffectsSettings.PlayDelay, () =>
            {
                _soundsPlayer.Play2DSound(_audioEffectsSettings.UnhoverSFX);
            });
        }

        public override void PointerClickListener(BaseEventData data = default)
        {
            // If the cooldown has not passed, ignore the click
            if (Time.unscaledTime - _lastClickTime < _animationData.ClickCooldownTime)
            {
                return;
            }

            _lastClickTime = Time.unscaledTime;

            _targetSettingsParams = _animationData.ClickAnimationParams;
            _targetSettingsParams.TargetTitleSize = _buttonText.fontSize * _animationData.ClickAnimationParams.TargetScale.x;
            _targetSettingsParams.TargetScale = transform.localScale * _animationData.ClickAnimationParams.TargetScale;

            ReplaceCurrentAnimation(PlayAnimation(_targetSettingsParams), true);
            CurrentAnimation.SetLoops(2, LoopType.Yoyo);

            _soundsPlayer.Play2DSound(_audioEffectsSettings.ClickSFX);
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
