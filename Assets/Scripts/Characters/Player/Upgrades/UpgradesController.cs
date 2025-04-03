
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Characters.Common.Combat.Weapons;
using Characters.Common.Levels;
using Cysharp.Threading.Tasks;
using Settings.Global;
using UI.Characters.Upgrades;
using UnityEngine;
using Utilities;
using Utilities.ErrorHandling;
using Zenject;

namespace Characters.Player.Upgrades
{
    public class UpgradesController : MonoBehaviour
    {
        #region Fields 

        private Dictionary<IUpgradableCharacterLogic, List<UpgradeProvider>> _availableCharactersUpgrades;
        private Dictionary<IUpgradableCharacterLogic, List<UpgradeProvider>> _waitingCharactersUpgrades;
        private IUpgradeHandlerUI _upgradeHandlerUI;

        private CharacterLevelUI _characterLevelUI;
        private GameStateService _gameStateService;

        private ushort _upgradesToProcessCount;
        private bool _levelUpdated;

        private UniTask _currentProcessTask = UniTask.CompletedTask;
        private CancellationTokenSource _cts;

        private bool _isNewUpgradeReceived = false;
        private UpgradeProvider _receivedUpgrade;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(
            PlayerCharacterController player,
            UpgradesSetData upgradesSetData,
            CharacterLevelUI characterLevelUI,
            IUpgradeHandlerUI upgradeHandler
            )
        {
            if (player.CharacterLogic is not IUpgradableCharacterLogic upgradableCharacter)
            {
                ErrorLogger.Log($"{gameObject.name} | Player character does not implement {nameof(IUpgradableCharacterLogic)} | Can not add upgrades | Abort.");
                return;
            }

            AddCharacterWithUpgrades(upgradableCharacter, upgradesSetData.UpgradesConfigurations);

            _characterLevelUI = characterLevelUI;
            _characterLevelUI.OnLevelNumberUpdate += CharacterLevelUpdateListener;

            _upgradeHandlerUI = upgradeHandler;
            _upgradeHandlerUI.OnUpgradeSelected += ReceiveUpgradeToApply;
        }

        private void Awake()
        {
            _gameStateService = ServiceLocator.Current.Get<GameStateService>();
        }

        private void OnDestroy()
        {
            StopAllUpgradesProcessing();

            _characterLevelUI.OnLevelNumberUpdate -= CharacterLevelUpdateListener;
            _upgradeHandlerUI.OnUpgradeSelected -= ReceiveUpgradeToApply;

            foreach (var characterInfo in _availableCharactersUpgrades)
            {
                characterInfo.Key.OnReadyForUpgrade -= UpgradeRequestListener;
                characterInfo.Key.OnSpecificTimeUpgradesRequested -= ConfigureTimeSpecificUpgrades;
            }

            _availableCharactersUpgrades = null;
        }

        #endregion

        public void AddCharacterWithUpgrades(IUpgradableCharacterLogic characterLogic, IEnumerable<UpgradeConfigurationSO> upgradeConfigurationsCollection)
        {
            TryInitCollections(characterLogic);

            UpgradeProvider upgradeProvider;
            List<UpgradeProvider> upgradeProvidersList = new List<UpgradeProvider>();

            foreach (var upgradeConfiguration in upgradeConfigurationsCollection)
            {
                upgradeProvider = new UpgradeProvider(upgradeConfiguration);
                upgradeProvidersList.Add(upgradeProvider);
            }

            foreach (var provider in upgradeProvidersList)
            {
                if (provider.AppearTime == UpgradeAppearTime.Any)
                {
                    _availableCharactersUpgrades[characterLogic].Add(provider);
                }
                else
                {
                    _waitingCharactersUpgrades[characterLogic].Add(provider);
                }
            }

            characterLogic.OnReadyForUpgrade += UpgradeRequestListener;
            characterLogic.OnSpecificTimeUpgradesRequested += ConfigureTimeSpecificUpgrades;
        }


        private void ConfigureTimeSpecificUpgrades(IUpgradableCharacterLogic targetCharacter, UpgradeAppearTime appearTime)
        {
            var waitingUpgrades = _waitingCharactersUpgrades.GetOrAdd(targetCharacter, () => new List<UpgradeProvider>());
            var availableUpgrades = _availableCharactersUpgrades.GetOrAdd(targetCharacter, () => new List<UpgradeProvider>());

            MoveUpgradesBasedOnTime(waitingUpgrades, availableUpgrades, appearTime, isMatching: true);  // Move matching upgrades from waiting to available
            MoveUpgradesBasedOnTime(availableUpgrades, waitingUpgrades, appearTime, isMatching: false); // Move non-matching upgrades from available to waiting
        }

        /// <summary>
        /// Moves upgrades from the source list to the target list based on the specified appearance time and condition.
        /// </summary>
        /// <param name="source">The source list from which upgrades will be moved.</param>
        /// <param name="target">The target list to which upgrades will be added.</param>
        /// <param name="appearTime">The time at which the upgrade is available (e.g., Day, Night, or Any).</param>
        /// <param name="isMatching">A flag indicating whether the condition should check for matching (true) or non-matching (false) appearance time.</param>
        private void MoveUpgradesBasedOnTime(List<UpgradeProvider> source, List<UpgradeProvider> target, UpgradeAppearTime appearTime, bool isMatching)
        {
            Predicate<UpgradeProvider> condition = isMatching
                ? (provider) => provider.AppearTime == appearTime || provider.AppearTime == UpgradeAppearTime.Any
                : (provider) => provider.AppearTime != appearTime && provider.AppearTime != UpgradeAppearTime.Any;

            var upgradesToMove = source.Where(provider => condition(provider)).ToList();

            if (upgradesToMove.Count() > 0)
            {
                target.AddRange(upgradesToMove);
                source.RemoveAll(upgradesToMove.Contains);
            }
        }

        private void TryInitCollections(IUpgradableCharacterLogic characterLogic)
        {
            _availableCharactersUpgrades ??= new();
            _waitingCharactersUpgrades ??= new();

            _availableCharactersUpgrades.TryAdd(characterLogic, new List<UpgradeProvider>());
            _waitingCharactersUpgrades.TryAdd(characterLogic, new List<UpgradeProvider>());
        }

        public void StopAllUpgradesProcessing()
        {
            if (_cts == null)
            {
                return;
            }

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }


        private void UpgradeRequestListener(IUpgradableCharacterLogic character, EntityLevelArgs level)
        {
            _upgradesToProcessCount += 1;

            if (_currentProcessTask.Status == UniTaskStatus.Pending)
            {
                return;
            }

            if ((_cts == null) || _cts.IsCancellationRequested)
            {
                _cts = new CancellationTokenSource();
            }

            // Future ToDo : update this logic to process upgrades for multiple players 
            _currentProcessTask = ProcessUpgradesAsync(character, _cts.Token);
        }


        private async UniTask ProcessUpgradesAsync(IUpgradableCharacterLogic character, CancellationToken token = default)
        {
            if (!_availableCharactersUpgrades.TryGetValue(character, out var upgradesProvidersList))
            {
                return;
            }

            while (_upgradesToProcessCount > 0)
            {
                await UniTask.WaitUntil(() => _levelUpdated, cancellationToken: token);
                _gameStateService.PauseHandler.TryPauseGame();
                _levelUpdated = false;

                if (token.IsCancellationRequested || upgradesProvidersList.Count == 0)
                {
                    _gameStateService.PauseHandler.TryUnpauseGame();
                    return;
                }

                var upgradesToDisplay = GetRandomUpgrades(upgradesProvidersList);
                _upgradeHandlerUI.DisplayUpgrades(upgradesToDisplay);

                var targetUpgrade = await WaitForUpgradeReceiveAsync(token);

                if (targetUpgrade != null)
                {
                    ApplyUpgrade(character, targetUpgrade);
                }

                _gameStateService.PauseHandler.TryUnpauseGame();
                _upgradesToProcessCount -= 1;
            }
        }

        private IEnumerable<UpgradeProvider> GetRandomUpgrades(List<UpgradeProvider> availableUpgrades)
        {
            int upgradesToTake = Mathf.Clamp(availableUpgrades.Count(), 0, 3);

            if (availableUpgrades.Count <= upgradesToTake)
            {
                return availableUpgrades;
            }

            HashSet<UpgradeProvider> randomUpgrades = new(upgradesToTake);
            int upgradeRandomIndex;
            UpgradeProvider targetUpgrade;

            while (randomUpgrades.Count < upgradesToTake)
            {
                upgradeRandomIndex = UnityEngine.Random.Range(0, availableUpgrades.Count());     // [0, available_count)
                targetUpgrade = availableUpgrades[upgradeRandomIndex];

                randomUpgrades.Add(targetUpgrade);
            }

            return randomUpgrades;
        }

        private void CharacterLevelUpdateListener(object sender, int currentLevel)
        {
            _levelUpdated = true;
        }

        private void ReceiveUpgradeToApply(object sender, UpgradeProvider targetUpgrade)
        {
            _isNewUpgradeReceived = true;
            _receivedUpgrade = targetUpgrade;
        }

        private async UniTask<UpgradeProvider> WaitForUpgradeReceiveAsync(CancellationToken token = default)
        {
            await UniTask.WaitUntil(() => _isNewUpgradeReceived, cancellationToken: token);
            _isNewUpgradeReceived = false;
            return _receivedUpgrade;
        }


        private void ApplyUpgrade(IUpgradableCharacterLogic targetCharacter, UpgradeProvider upgradeProvider)
        {
            if (!upgradeProvider.HasNextLevel)
            {
                ErrorLogger.LogWarning("# Upgrade Provider can not provide upgrade level! It must be removed before! - " + gameObject.name);
            }

            switch (upgradeProvider.Type)
            {
                case UpgradeType.Character:
                    HandleCharacterUpgrade(targetCharacter, upgradeProvider);
                    break;

                case UpgradeType.AbilityUnlock:
                    HandleAbilityUnlock(targetCharacter, upgradeProvider);
                    break;

                case UpgradeType.Ability:
                    HandleAbilityUpgrade(targetCharacter, upgradeProvider);
                    break;
            }

            if (!upgradeProvider.HasNextLevel)
            {
                _availableCharactersUpgrades[targetCharacter].Remove(upgradeProvider);
            }
        }

        private void HandleCharacterUpgrade(IUpgradableCharacterLogic targetCharacter, UpgradeProvider upgradeProvider)
        {
            targetCharacter.UpgradesCoordinator.ApplyCharacterUpgrade(upgradeProvider.GetNextUpgradeLevel<UpgradeLevelSO<IUpgradableCharacterLogic>>());
        }

        private void HandleAbilityUpgrade(IUpgradableCharacterLogic targetCharacter, UpgradeProvider upgradeProvider)
        {
            if (upgradeProvider is WeaponUpgradeProvider weaponUpgradeProvider)
            {
                targetCharacter.UpgradesCoordinator.ApplyWeaponUpgrade(weaponUpgradeProvider.TargetWeaponID, weaponUpgradeProvider.GetNextUpgradeLevel<UpgradeLevelSO<IUpgradableWeapon>>());
            }
            else
            {
                ErrorLogger.LogWarning($"Warning | Wrong type of upgrade provider | Can not upgrade the ability, deleting it.. | {gameObject.name}");
                _availableCharactersUpgrades[targetCharacter].Remove(upgradeProvider);
            }
        }

        private void HandleAbilityUnlock(IUpgradableCharacterLogic targetCharacter, UpgradeProvider upgradeProvider)
        {
            var abilityUnlockLevel = upgradeProvider.GetNextUpgradeLevel<WeaponUnlockLevelSO>();
            targetCharacter.UpgradesCoordinator.UnlockAbility(abilityUnlockLevel);

            // Add a new Upgrade Configuration from unlocked ability.

            SingleWeaponUnlockSO singleWeaponUnlock = null;

            foreach (var singleUpgrade in abilityUnlockLevel.Upgrades)
            {
                singleWeaponUnlock = (singleUpgrade as SingleWeaponUnlockSO);

                if (singleWeaponUnlock != null)
                {
                    // Note : give the weapon ID to find the target weapon (if it is needed) when applying the upgrade.
                    AddUpgradeConfiguration(targetCharacter, singleWeaponUnlock.WeaponUpgradeConfiguration, singleWeaponUnlock.WeaponID);
                }
            }
        }

        private void AddUpgradeConfiguration(IUpgradableCharacterLogic targetCharacter, UpgradeConfigurationSO upgradeConfigurationSO, int extraTargetID = -1)
        {
            UpgradeProvider upgradeProvider = null;

            if (upgradeConfigurationSO.Upgrade is WeaponUpgradeSO)
            {
                upgradeProvider = new WeaponUpgradeProvider(upgradeConfigurationSO, extraTargetID);
            }
            else
            {
                upgradeProvider = new UpgradeProvider(upgradeConfigurationSO);
            }

            _availableCharactersUpgrades[targetCharacter]?.Add(upgradeProvider);
        }
    }

    #endregion
}

