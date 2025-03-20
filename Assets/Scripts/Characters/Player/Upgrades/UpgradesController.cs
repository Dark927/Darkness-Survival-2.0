
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Characters.Common.Levels;
using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using Settings.Global;
using UI.Characters.Upgrades;
using UnityEngine;
using Zenject;

namespace Characters.Player.Upgrades
{
    public class UpgradesController : MonoBehaviour
    {
        #region Fields 

        [SerializeField] private List<UpgradeConfigurationSO> _characterUpgradeDatas;

        // ToDo : Create some container to hold UpgradeConfigurationSO and handle its levels
        private Dictionary<ICharacterLogic, List<UpgradeConfigurationSO>> _charactersUpgrades;
        private IUpgradeHandlerUI _upgradeHandlerUI;

        private CharacterLevelUI _characterLevelUI;
        private GameStateService _gameStateService;

        private ushort _upgradesToProcessCount;
        private bool _levelUpdated;

        private UniTask _currentProcessTask = UniTask.CompletedTask;
        private CancellationTokenSource _cts;

        private bool _isNewUpgradeReceived = false;
        private UpgradeConfigurationSO _receivedUpgrade;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(PlayerCharacterController player, CharacterLevelUI characterLevelUI, IUpgradeHandlerUI upgradeHandler)
        {
            AddCharacterWithUpgrades(player.CharacterLogic, _characterUpgradeDatas);

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

        public void AddCharacterWithUpgrades(ICharacterLogic characterLogic, List<UpgradeConfigurationSO> upgrades)
        {
            _charactersUpgrades ??= new();
            _charactersUpgrades.Add(characterLogic, upgrades);

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
            if (!_charactersUpgrades.TryGetValue(character, out var upgradesDataList))
            {
                return;
            }


            while (_upgradesToProcessCount > 0)
            {
                await UniTask.WaitUntil(() => _levelUpdated, cancellationToken: token);
                _gameStateService.PauseHandler.TryPauseGame();

                if (token.IsCancellationRequested)
                {
                    _gameStateService.PauseHandler.TryUnpauseGame();
                    return;
                }

                _levelUpdated = false;

                // ToDo : take random upgrades here
                _upgradeHandlerUI.DisplayUpgrades(upgradesDataList.Take(3));

                var targetUpgrade = await WaitForUpgradeReceiveAsync(token);

                if (targetUpgrade != null)
                {
                    ApplyUpgrade(character, targetUpgrade);
                }

                _gameStateService.PauseHandler.TryUnpauseGame();
                _upgradesToProcessCount -= 1;
            }
        }

        private void CharacterLevelUpdateListener(object sender, int currentLevel)
        {
            _levelUpdated = true;
        }

        private void ReceiveUpgradeToApply(object sender, UpgradeConfigurationSO targetUpgrade)
        {
            _isNewUpgradeReceived = true;
            _receivedUpgrade = targetUpgrade;
        }

        private async UniTask<UpgradeConfigurationSO> WaitForUpgradeReceiveAsync(CancellationToken token = default)
        {
            await UniTask.WaitUntil(() => _isNewUpgradeReceived, cancellationToken: token);
            _isNewUpgradeReceived = false;
            return _receivedUpgrade;
        }

        // ToDo : take next levels of upgrades here, so use custom container.
        private void ApplyUpgrade(ICharacterLogic targetCharacter, UpgradeConfigurationSO upgradeData)
        {
            CharacterUpgradeSO characterUpgrade = upgradeData.Upgrade as CharacterUpgradeSO;
            var upgradeLevel = characterUpgrade.UpgradeLevels.First();
            targetCharacter.ApplyUpgrade(upgradeLevel);
        }

        #endregion
    }
}
