using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultKeyboard : IKeyboardLayout
{
    private PlayerInputActions _inputActions;
    private InputAction _playerMovement;

    public DefaultKeyboard()
    {
        InitInputActions();
    }

    private void InitInputActions()
    {
        _inputActions = new PlayerInputActions();
        _playerMovement = _inputActions.PlayerActions.Movement;
    }
    
    public void EnableMovementInput()
    {
        _playerMovement.Enable();
    }

}
