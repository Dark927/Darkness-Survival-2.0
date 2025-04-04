
using System;
using Characters.Player.Upgrades;
using Settings.Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;
using Utilities.ErrorHandling;

namespace UI.Characters.Upgrades
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UpgradeCardUI : MonoBehaviour, IInitializable
    {
        #region Events 

        public event Action<UpgradeCardUI> OnCardSelected;

        #endregion

        #region Fields 

        [CustomHeader("Body", 2, 0)]
        [SerializeField] private Image _upgradeCard;

        [CustomHeader("Header", 2, 0)]
        [SerializeField] private TextMeshProUGUI _upgradeTitleText;
        [SerializeField] private TextMeshProUGUI _upgradeTypeText;

        [Space, CustomHeader("Icon", 1, 0)]
        [SerializeField] private Image _upgradeIcon;

        [Space, CustomHeader("Details", 1, 0)]
        [SerializeField] private TextMeshProUGUI _upgradeLevelText;
        [SerializeField] private TextMeshProUGUI _upgradeDescriptionText;

        private CardAnimationsUI _animations;
        private CanvasGroup _canvasGroup;

        private UpgradeCardVisualSettings _initialVisualSettings;

        #endregion


        #region Properties

        public CardAnimationsUI Animations => _animations;

        #endregion


        #region Methods

        #region Init

        public void Initialize()
        {
            _animations = GetComponent<CardAnimationsUI>();

            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;

            _initialVisualSettings = new(_upgradeTitleText.color, _upgradeCard.color, _upgradeIcon.color);
        }


        #region Editor 

        private void OnValidate()
        {
            if (_upgradeCard == null)
            {
                ErrorLogger.LogWarning($"{gameObject.name} | {this.GetType()} | Upgrade card image is null!");
            }

            if (_upgradeTitleText == null)
            {
                ErrorLogger.LogWarning($"{gameObject.name} | {this.GetType()} | Upgrade card title text is null!");
            }

            if (_upgradeTypeText == null)
            {
                ErrorLogger.LogWarning($"{gameObject.name} | {this.GetType()} | Upgrade card upgrade type text is null!");
            }

            if (_upgradeIcon == null)
            {
                ErrorLogger.LogWarning($"{gameObject.name} | {this.GetType()} | Upgrade card icon is null!");
            }

            if (_upgradeDescriptionText == null)
            {
                ErrorLogger.LogWarning($"{gameObject.name} | {this.GetType()} | Upgrade card upgrade description text is null!");
            }
        }

        #endregion

        #endregion

        public void Show(Action callback = null)
        {
            if (_animations != null)
            {
                _animations.PrepareAnimation();
                _animations.Show(() =>
                {
                    _animations.UnblockPointerInteraction();
                    callback?.Invoke();
                }
                );
                return;
            }

            _canvasGroup.alpha = 1;
            callback?.Invoke();
        }

        public void Hide(Action callback = null)
        {
            void FinishHideActions()
            {
                SetMainVisualSettings(_initialVisualSettings);
                gameObject.SetActive(false);
            }

            if (_animations != null)
            {
                _animations.BlockPointerInteraction();

                _animations.Hide(() =>
                {
                    callback?.Invoke();
                    FinishHideActions();
                });

                return;
            }

            callback?.Invoke();
            _canvasGroup.alpha = 0;
            FinishHideActions();
        }

        public void SetUpgradeInfo(UpgradeInfoUI upgradeInfo)
        {
            _upgradeTitleText.text = upgradeInfo.Title;
            _upgradeTypeText.text = upgradeInfo.Type;
            _upgradeIcon.sprite = upgradeInfo.Icon;
            _upgradeLevelText.text = upgradeInfo.Level;
            _upgradeDescriptionText.text = upgradeInfo.Description;
        }

        public void SetUpgradeVisual(UpgradeVisualDataUI visualData)
        {
            if (visualData == null)
            {
                return;
            }

            SetMainVisualSettings(visualData.MainVisualSettings);

            if ((Animations != null) && (visualData.AnimationsData != null))
            {
                Animations.SetAnimationData(visualData.AnimationsData);
            }
        }

        private void SetMainVisualSettings(UpgradeCardVisualSettings visualSettings)
        {
            _upgradeTitleText.color = visualSettings.TitleColor;
            _upgradeCard.color = visualSettings.CardColor;
            _upgradeIcon.color = visualSettings.IconTint;
        }

        #region Pointer Actions 

#pragma warning disable IDE0060 // Remove unused parameter

        public void PointerClickListener()
        {
            OnCardSelected?.Invoke(this);
        }

#pragma warning restore IDE0060 // Remove unused parameter

        #endregion

        #endregion
    }
}
