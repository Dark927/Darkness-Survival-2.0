using Settings.Global;
using UnityEngine.InputSystem;
using static IControlLayout;

namespace Characters.Player.Controls
{
    public class DefaultControlLayout : IControlLayout
    {
        #region Fields 

        private PlayerInputActions _inputActions;
        private InputAction _playerMovement;
        private PlayerBasicAttacks _playerBasicAttacks;

        #endregion


        #region Properties

        public InputAction PlayerMovement { get => _playerMovement; private set => _playerMovement = value; }
        public PlayerBasicAttacks PlayerAttacks { get => _playerBasicAttacks; private set => _playerBasicAttacks = value; }

        #endregion


        #region Methods

        public DefaultControlLayout()
        {
            InitInputActions();
            EnableInputs();
        }

        public void EnableInputs()
        {
            _inputActions.Enable();
        }

        public void DisableInputs()
        {
            _inputActions.Disable();
        }

        private void InitInputActions()
        {
            _inputActions = ServiceLocator.Current.Get<InputService>().InputActions;
            _playerMovement = _inputActions.PlayerActions.Movement;
            _playerBasicAttacks = new PlayerBasicAttacks(_inputActions.PlayerActions.FastAttack, _inputActions.PlayerActions.HeavyAttack);
        }

        #endregion
    }
}
