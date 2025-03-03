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

    public override void ClickListener()
    {
        _pauseService.PauseGame();
        GameplayUI.Instance.ActivatePauseMenu();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_player == null)
        {
            PlayerService playerService = ServiceLocator.Current.Get<PlayerService>();
            _player = playerService.Players.FirstOrDefault();
        }

        _player.SetCanAttackFlag(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_player == null)
        {
            PlayerService playerService = ServiceLocator.Current.Get<PlayerService>();
            _player = playerService.Players.FirstOrDefault();
        }

        _player.SetCanAttackFlag(true);
    }
}
