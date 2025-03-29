using DG.Tweening;
using TMPro;
using UI.CustomCursor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.Attributes;
using Utilities.ErrorHandling;

namespace UI.Characters.Upgrades
{
    public class CardAnimationsUI : PopupUI
    {
        #region Fields 

        [Space, CustomHeader("Card Main", 4, 0, CustomHeaderAttribute.HeaderColor.green)]

        [SerializeField] private CardAnimationDataUI _animationData;
        [SerializeField] private TextMeshProUGUI _cardTitle;
        [SerializeField] private Image _cardImage;
        [SerializeField] private Image _cardIcon;

        private CardAnimationParamsUI _targetSettingsParams;
        private CardAnimationParamsUI _startSettingsParams;

        private bool _blockPointerInteraction;

        #endregion


        #region Properties

        public bool IsInteractionBlocked => _blockPointerInteraction;

        #endregion



        #region Methods

        #region Init

        protected override void Awake()
        {
            base.Awake();

            _blockPointerInteraction = true;

            if (_animationData == null || _cardTitle == null || _cardImage == null)
            {
                ErrorLogger.LogWarning($"{gameObject.name} | {nameof(CardAnimationDataUI)} | One of important components is null! Disabling component..");
                gameObject.SetActive(false);
                return;
            }

            _startSettingsParams.TargetScale = transform.localScale;
        }

        public void SetAnimationData(CardAnimationDataUI animationData)
        {
            _animationData = animationData;
            _startSettingsParams.DurationInSec = _animationData.HoverAnimationParams.DurationInSec;

            _startSettingsParams.TargetTitleColor = _cardTitle.color;
            _startSettingsParams.TargetCardColor = _cardImage.color;
            _startSettingsParams.TargetCardIconColor = _cardIcon.color;
        }

        #endregion

        ////////
        // Listeners for Event Trigger configured with the Unity Inspector 
        ////////

#pragma warning disable IDE0060 // Remove unused parameter

        public override void PointerEnterListener(BaseEventData data = default)
        {
            MouseCursor.Instance.HoverInteractiveUI();

            if (_blockPointerInteraction || _animationData == null)
            {
                return;
            }

            _targetSettingsParams = _animationData.HoverAnimationParams;
            _targetSettingsParams.TargetScale *= _startSettingsParams.TargetScale;
            transform.SetAsLastSibling();
            ReplaceCurrentAnimation(PlayAnimation(_targetSettingsParams), false);
        }

        public override void PointerExitListener(BaseEventData data = default)
        {
            if (_blockPointerInteraction)
            {
                return;
            }

            MouseCursor.Instance.ExitInteractiveUI();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            ReplaceCurrentAnimation(PlayAnimation(_startSettingsParams), false);
        }

#pragma warning restore IDE0060 // Remove unused parameter

        ////////

        public void BlockPointerInteraction() => _blockPointerInteraction = true;
        public void UnblockPointerInteraction() => _blockPointerInteraction = false;

        public Sequence PlayClickAnimation()
        {
            if (_blockPointerInteraction || _animationData == null)
            {
                return null;
            }

            _targetSettingsParams = _animationData.ClickAnimationParams;
            _targetSettingsParams.TargetScale *= _startSettingsParams.TargetScale;

            BlockPointerInteraction();
            MouseCursor.Instance.ExitInteractiveUI();

            ReplaceCurrentAnimation(PlayAnimation(_targetSettingsParams), true);
            CurrentAnimation.SetLoops(2, LoopType.Yoyo);

            CurrentAnimation.OnComplete(() => UnblockPointerInteraction());

            return CurrentAnimation;
        }

        public override void PrepareAnimation()
        {
            base.PrepareAnimation();


            _blockPointerInteraction = true;
        }

        private Sequence PlayAnimation(CardAnimationParamsUI parameters)
        {
            Sequence animation = DOTween.Sequence();

            animation
                .Append(
                    _cardTitle
                        .DOColor(parameters.TargetTitleColor, parameters.DurationInSec)
                        .From(_cardTitle.color)
                )
                .Join(
                    transform
                        .DOScale(parameters.TargetScale, parameters.DurationInSec)
                        .From(transform.localScale)
                )
                .Join(
                    _cardImage
                        .DOColor(parameters.TargetCardColor, parameters.DurationInSec)
                        .From(_cardImage.color)
                )
                .Join(
                    _cardIcon
                        .DOColor(parameters.TargetCardIconColor, parameters.DurationInSec)
                        .From(_cardIcon.color)
                );

            animation.SetUpdate(true);
            return animation;
        }


        #endregion
    }
}
