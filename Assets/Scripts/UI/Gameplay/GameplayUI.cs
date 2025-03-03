using Characters.Player;
using Settings.Abstract;
using Settings.AssetsManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public sealed class GameplayUI : SingletonMonoBase<GameplayUI>
{
    private PlayerCharacterController _targetCharacter;
    [SerializeField] private Canvas _menuCanvas;

    [Header("Panels")]
    [SerializeField] private AssetReference _gameOverPanel;
    [SerializeField] private AssetReference _pauseMenuAsset;

    private AsyncOperationHandle<GameObject> _pauseMenuLoadHandle;
    private MenuUI _pauseMenu;
    private bool _isPauseMenuActivated = false;


    private void Awake()
    {
        InitInstance();
    }

    public void Initialize(PlayerCharacterController targetPlayer)
    {
        // ToDo : Move this logic to the another component!
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
            _pauseMenu = pauseMenuObject.GetComponent<MenuUI>();
        }
    }

    public void DeactivatePauseMenu()
    {
        if (_isPauseMenuActivated && (_pauseMenu != null))
        {
            Destroy(_pauseMenu.gameObject);
            AddressableAssetsLoader.Instance.TryUnloadAsset(_pauseMenuLoadHandle);
            _isPauseMenuActivated = false;
        }
    }
}