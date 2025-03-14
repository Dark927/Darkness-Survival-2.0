using System;

namespace Characters.Player.Controls
{
    public class GamePlayerInputHandler
    {
        private PlayerCharacterController _player;

        public GamePlayerInputHandler(PlayerCharacterController player)
        {
            _player = player;
        }

        public void TryBlockCharacterInput()
        {
            DoPlayersAction(player => player.Input.DeactivateInput());
        }

        public void TryUnblockCharacterInput()
        {
            DoPlayersAction(player => player.Input.ActivateInput());
        }

        private void DoPlayersAction(Action<PlayerCharacterController> action)
        {
            if (_player == null)
            {
                return;
            }

            action?.Invoke(_player);
        }
    }
}
