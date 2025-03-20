using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Player.Upgrades;
using Settings.Global;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.ErrorHandling;

namespace UI.Characters.Upgrades
{
    public class UpgradeCardsHandlerUI : MonoBehaviour, IUpgradeHandlerUI
    {
        #region Events

        public event EventHandler<UpgradeConfigurationSO> OnUpgradeSelected;

        #endregion


        #region Fields 

        [SerializeField] private float _selectedCardHideDelay = 0.75f;

        private const int MaxUpgradesAtOnce = 3;
        private UpgradeCardsHolderUI _cardsHolder;

        private Dictionary<UpgradeCardUI, UpgradeConfigurationSO> _configuredCards;
        private StagePostProcessService _stagePostProcessService;
        private CharacterLevelUI _characterLevelUI;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            _cardsHolder = GetComponentInChildren<UpgradeCardsHolderUI>();
            _configuredCards = new();
        }

        ////////////////////////////////////////////////////////////////
        // ToDo : remove this 
        // ToDo : remove this 
        // ToDo : remove this 
        ////////////////////////////////////////////////////////////////

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                HideUpgrades();
            }

            if (Keyboard.current.leftAltKey.wasPressedThisFrame)
            {
                DisplayUpgrades(_upgradesData);
            }
        }
        IEnumerable<UpgradeConfigurationSO> _upgradesData;

        ////////////////////////////////////////////////////////////////

        #endregion

        public void DisplayUpgrades(IEnumerable<UpgradeConfigurationSO> upgradesData)
        {
            // temp
            _upgradesData = upgradesData;

            int upgradesCount = upgradesData.Count();

            if (upgradesCount == 0)
            {
                return;
            }

            upgradesCount = Mathf.Clamp(upgradesCount, 0, MaxUpgradesAtOnce);

            _configuredCards = ConfigureUpgradesCards(upgradesData, upgradesCount);
            ActivateUpgradeCards(_configuredCards);
        }

        public void HideUpgrades()
        {
            foreach (var card in _configuredCards)
            {
                card.Key.Hide(() => card.Key.gameObject.SetActive(false));
            }

            _configuredCards.Clear();
        }

        private void ActivateUpgradeCards(Dictionary<UpgradeCardUI, UpgradeConfigurationSO> upgradeCards)
        {
            _stagePostProcessService ??= ServiceLocator.Current.Get<StagePostProcessService>();
            var upgradeCard = upgradeCards.FirstOrDefault().Key;

            if (upgradeCard.Animations != null)
            {
                float duration = upgradeCard.Animations.ScaleDuration;
                _stagePostProcessService?.Grayscale?.ApplyGrayscale(duration);
            }

            foreach (var cardInfo in upgradeCards)
            {
                cardInfo.Key.gameObject.SetActive(true);
                cardInfo.Key.Show();
                cardInfo.Key.OnCardSelected += CardSelectedListener;
            }

            _cardsHolder.UpdateLayout();
        }

        private static void ProcessUnselectedCard(KeyValuePair<UpgradeCardUI, UpgradeConfigurationSO> cardInfo)
        {
            cardInfo.Key.Hide();
        }

        private void ProcessSelectedCard(KeyValuePair<UpgradeCardUI, UpgradeConfigurationSO> cardInfo)
        {
            CardAnimationsUI cardAnimations = cardInfo.Key.Animations;
            Action notifyUpgradeSelected = () => OnUpgradeSelected?.Invoke(this, cardInfo.Value);

            if (cardAnimations != null)
            {
                cardAnimations.PlayClickAnimation();
                cardAnimations.DoDelayedActions(_selectedCardHideDelay, () =>
                    {
                        cardInfo.Key.Hide(notifyUpgradeSelected);
                        _stagePostProcessService?.Grayscale?.RemoveGrayscale(cardAnimations.ScaleDuration * cardAnimations.DeactivationSpeedMult);
                    });
            }
            else
            {
                cardInfo.Key.Hide(notifyUpgradeSelected);
            }
        }

        private Dictionary<UpgradeCardUI, UpgradeConfigurationSO> ConfigureUpgradesCards(IEnumerable<UpgradeConfigurationSO> upgradesData, int targetUpgradesCount)
        {
            List<UpgradeCardUI> availableCards = _cardsHolder.Cards.ToList();

            if (availableCards.Count == 0)
            {
                return null;
            }

            Dictionary<UpgradeCardUI, UpgradeConfigurationSO> configuredCards = new();
            UpgradeCardUI targetCard;
            UpgradeConfigurationSO currentUpgradeData;
            UpgradeInfoUI currentUpgradeInfo;

            int availableCardsCount = availableCards.Count;

            for (int currentUpgrade = 0; (currentUpgrade < targetUpgradesCount) && (currentUpgrade < availableCardsCount); ++currentUpgrade)
            {
                targetCard = availableCards.ElementAt(currentUpgrade);
                currentUpgradeData = upgradesData.ElementAt(currentUpgrade);

                if (currentUpgradeData == null)
                {
                    continue;
                }

                currentUpgradeInfo = ConvertUpgradeDataToInfo(currentUpgradeData);

                targetCard.SetUpgradeInfo(currentUpgradeInfo);
                targetCard.SetUpgradeVisual(currentUpgradeData.VisualData);

                configuredCards.Add(targetCard, currentUpgradeData);
            }

            return configuredCards;
        }

        private UpgradeInfoUI ConvertUpgradeDataToInfo(UpgradeConfigurationSO upgradeData)
        {
            return new UpgradeInfoUI()
            {
                Title = upgradeData.Name,
                Type = upgradeData.Type.ToString(),
                Description = upgradeData.Description,
                Icon = upgradeData.Icon,
            };
        }

        private void LevelNumberUpdateListener(object sender, int level)
        {
            if (_configuredCards.Count == 0)
            {
                return;
            }
            ActivateUpgradeCards(_configuredCards);
        }

        private void CardSelectedListener(UpgradeCardUI selectedCard)
        {
            if (!_configuredCards.ContainsKey(selectedCard))
            {
                ErrorLogger.LogWarning($"{gameObject.name} " +
                    $"| {nameof(UpgradeCardsHandlerUI)} " +
                    $"| Unexpected behaviour, selected card is not in the list of configured ones.");
                return;
            }

            var chosenUpgrade = _configuredCards[selectedCard];

            foreach (var cardInfo in _configuredCards)
            {
                cardInfo.Key.OnCardSelected -= CardSelectedListener;

                if (cardInfo.Key != selectedCard)
                {
                    ProcessUnselectedCard(cardInfo);
                    continue;
                }

                ProcessSelectedCard(cardInfo);
            }
            _configuredCards.Clear();
        }

        #endregion
    }
}
