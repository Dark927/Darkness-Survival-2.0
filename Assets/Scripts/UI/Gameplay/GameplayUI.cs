using System;
using Characters.Player;
using Settings.Abstract;
using Settings.AssetsManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UI
{
    public sealed class GameplayUI : SingletonMonoBase<GameplayUI>
    {
        private PlayerCharacterController _targetCharacter;
        [SerializeField] private Canvas _menuCanvas;

        [Header("Panels")]
        [SerializeField] private AssetReference _gameOverPanel;
        [SerializeField] private AssetReference _pauseMenuAsset;

        private AsyncOperationHandle<GameObject> _pauseMenuLoadHandle;
        private MenuWithVideoUI _pauseMenu;
        private bool _isPauseMenuActivated = false;


        private void Awake()
        {
            InitInstance();
        }

        public void Initialize(PlayerCharacterController targetPlayer)
        {
            _targetCharacter = targetPlayer;
            _targetCharacter.OnCharacterDeathEnd += ActivateGameOverPanel;
        }

        private void OnDestroy()
        {
            if (_targetCharacter != null)
            {
                _targetCharacter.OnCharacterDeathEnd -= ActivateGameOverPanel;
            }
        }

        public void ActivateGameOverPanel()
        {
            //_gameOverPanel.SetActive(true);
        }

        public async void ActivatePauseMenu()
        {
            if (_isPauseMenuActivated)
            {
                return;
            }

            _isPauseMenuActivated = true;

            _pauseMenuLoadHandle = AddressableAssetsLoader.Instance.TryLoadAssetAsync<GameObject>(_pauseMenuAsset);
            await _pauseMenuLoadHandle.Task;

            if (_pauseMenuLoadHandle.Task.Result != null)
            {
                GameObject pauseMenuObject = Instantiate(_pauseMenuLoadHandle.Result, _menuCanvas.transform.position, Quaternion.identity, _menuCanvas.transform);
                _pauseMenu = pauseMenuObject.GetComponent<MenuWithVideoUI>();
                _pauseMenu.Activate();
            }
        }

        public void DeactivatePauseMenu(Action callback = default)
        {
            if (!_isPauseMenuActivated || (_pauseMenu == null))
            {
                return;
            }

            _pauseMenu.Deactivate(() =>
            {
                Destroy(_pauseMenu.gameObject);
                AddressableAssetsLoader.Instance.TryUnloadAsset(_pauseMenuLoadHandle);
                _isPauseMenuActivated = false;

                callback?.Invoke();
            });
        }
    }
}
