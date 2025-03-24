
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Characters.Common.Combat.Weapons;
using Characters.Common.Levels;
using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using Settings.Global;
using UI.Characters.Upgrades;
using UnityEngine;
using Utilities.ErrorHandling;
using Zenject;

namespace Characters.Player.Upgrades
{
    public class UpgradesController : MonoBehaviour
    {
        #region Fields 

        private Dictionary<ICharacterLogic, List<UpgradeProvider>> _charactersUpgrades;
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
            AddCharacterWithUpgrades(player.CharacterLogic, upgradesSetData.UpgradesConfigurations);

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

            foreach (var characterInfo in _charactersUpgrades)
            {
                characterInfo.Key.OnReadyForUpgrade -= UpgradeRequestListener;
            }

            _charactersUpgrades = null;
        }

        #endregion

        public void AddCharacterWithUpgrades(ICharacterLogic characterLogic, IEnumerable<UpgradeConfigurationSO> upgradeConfigurationsCollection)
        {
            _charactersUpgrades ??= new();

            UpgradeProvider upgradeProvider;
            List<UpgradeProvider> upgradeProvidersList = new List<UpgradeProvider>();

            foreach (var upgradeConfiguration in upgradeConfigurationsCollection)
            {
                upgradeProvider = new UpgradeProvider(upgradeConfiguration);
                upgradeProvidersList.Add(upgradeProvider);
            }

            _charactersUpgrades.Add(characterLogic, upgradeProvidersList);
            characterLogic.OnReadyForUpgrade += UpgradeRequestListener;
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

        private void UpgradeRequestListener(ICharacterLogic character, EntityLevelArgs level)
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

        private async UniTask ProcessUpgradesAsync(ICharacterLogic character, CancellationToken token = default)
        {
            if (!_charactersUpgrades.TryGetValue(character, out var upgradesProvidersList))
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
                upgradeRandomIndex = Random.Range(0, availableUpgrades.Count());     // [0, available_count)
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


        private void ApplyUpgrade(ICharacterLogic targetCharacter, UpgradeProvider upgradeProvider)
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
                _charactersUpgrades[targetCharacter].Remove(upgradeProvider);
            }
        }

        private void HandleCharacterUpgrade(ICharacterLogic targetCharacter, UpgradeProvider upgradeProvider)
        {
            targetCharacter.Upgrades.ApplyCharacterUpgrade(upgradeProvider.GetNextUpgradeLevel<UpgradeLevelSO<ICharacterLogic>>());
        }

        private void HandleAbilityUpgrade(ICharacterLogic targetCharacter, UpgradeProvider upgradeProvider)
        {
            if (upgradeProvider is WeaponUpgradeProvider weaponUpgradeProvider)
            {
                targetCharacter.Upgrades.ApplyWeaponUpgrade(weaponUpgradeProvider.TargetWeaponID, weaponUpgradeProvider.GetNextUpgradeLevel<UpgradeLevelSO<IWeapon>>());
            }
            else
            {
                ErrorLogger.LogWarning($"Warning | Wrong type of upgrade provider | Can not upgrade the ability, deleting it.. | {gameObject.name}");
                _charactersUpgrades[targetCharacter].Remove(upgradeProvider);
            }
        }

        private void HandleAbilityUnlock(ICharacterLogic targetCharacter, UpgradeProvider upgradeProvider)
        {
            var abilityUnlockLevel = upgradeProvider.GetNextUpgradeLevel<WeaponUnlockLevelSO>();
            targetCharacter.Upgrades.UnlockAbility(abilityUnlockLevel);

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

        private void AddUpgradeConfiguration(ICharacterLogic targetCharacter, UpgradeConfigurationSO upgradeConfigurationSO, int extraTargetID = -1)
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

            _charactersUpgrades[targetCharacter]?.Add(upgradeProvider);
        }
    }

    #endregion
}

