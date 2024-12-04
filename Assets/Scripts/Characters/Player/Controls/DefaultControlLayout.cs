using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultControlLayout : IControlLayout
{
    private PlayerInputActions _inputActions;
    private InputAction _playerMovement;

    public InputAction PlayerMovement { get => _playerMovement; private set => _playerMovement = value; }

    public DefaultControlLayout()
    {
        InitInputActions();
        EnableMovementInput();
    }

    public void EnableMovementInput()
    {
        _playerMovement.Enable();
    }

    private void InitInputActions()
    {
        _inputActions = new PlayerInputActions();
        _playerMovement = _inputActions.PlayerActions.Movement;
    }
}
