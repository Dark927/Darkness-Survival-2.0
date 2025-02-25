using Characters.Player;
using Settings.Abstract;
using Settings.Global;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public sealed class GameplayUI : SingletonBase<GameplayUI>
{
    private PlayerCharacterController _targetCharacter;

    [Header("Panels")]
    [SerializeField] private AssetReference _gameOverPanel;



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


}