using System.Linq;
using Characters.Player;
using Gameplay.Components;
using Settings.Global;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.Buttons
{
    public class SwitchPauseButtonUI : ButtonBaseUI, IPointerEnterHandler, IPointerExitHandler
    {
        private GameStateService _gameStateService;
        private PlayerCharacterController _player;

        protected override void Awake()
        {
            base.Awake();
            _gameStateService = ServiceLocator.Current.Get<GameStateService>();
        }

        public override void Click()
        {
            _gameStateService.SwitchPauseState();
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
}
