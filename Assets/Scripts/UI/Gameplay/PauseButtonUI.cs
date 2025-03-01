using Characters.Player;
using Settings.Global;
using System.Diagnostics;
using System.Linq;
using UI.Buttons;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.ErrorHandling;
using Zenject;

public class PauseButtonUI : ButtonBaseUI, IPointerEnterHandler, IPointerExitHandler
{
    private GamePauseService _pauseService;
    private PlayerCharacterController _player;

    [Inject]
    public void Construct(GamePauseService pauseService)
    {
        _pauseService = pauseService;
    }

    private void Start()
    {
        PlayerService playerService = ServiceLocator.Current.Get<PlayerService>();

        if (playerService != null)
        {
            _player = playerService.Players.FirstOrDefault();
        }
    }

    public override void ClickListener()
    {
        _pauseService.PauseGame();
        GameplayUI.Instance.ActivatePauseMenu();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_player == null)
        {
            ErrorLogger.LogComponentIsNull(LogOutputType.Console, gameObject.name, nameof(PlayerCharacterController));
            return;
        }

        _player.SetCanAttackFlag(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_player == null)
        {
            ErrorLogger.LogComponentIsNull(LogOutputType.Console, gameObject.name, nameof(PlayerCharacterController));
            return;
        }

        _player.SetCanAttackFlag(true);
    }
}
