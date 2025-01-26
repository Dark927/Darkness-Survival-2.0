using UnityEngine.InputSystem;

namespace Characters.Player.Controls
{
    public class DefaultControlLayout : IControlLayout
    {
        #region Fields 

        private PlayerInputActions _inputActions;
        private InputAction _playerMovement;
        private InputAction _playerBasicAttacks;

        #endregion


        #region Properties

        public InputAction PlayerMovement { get => _playerMovement; private set => _playerMovement = value; }
        public InputAction PlayerBasicAttacks { get => _playerBasicAttacks; private set => _playerBasicAttacks = value; }

        #endregion


        #region Methods

        public DefaultControlLayout()
        {
            InitInputActions();
            EnableInputs();
        }

        public void EnableInputs()
        {
            _playerMovement.Enable();
            _playerBasicAttacks.Enable();
        }

        public void DisableInputs()
        {
            _playerMovement.Disable();
            _playerBasicAttacks.Disable();
        }

        private void InitInputActions()
        {
            _inputActions = new PlayerInputActions();
            _playerMovement = _inputActions.PlayerActions.Movement;
            _playerBasicAttacks = _inputActions.PlayerActions.Attacks;
        }

        #endregion
    }
}