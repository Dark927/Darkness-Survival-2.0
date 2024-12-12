using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{
    private InputHandler _inputHandler;
    private PlayerMovement _playerMovement;

    public PlayerInput(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
    }

    public void SetMovement(ICharacterMovement playerMovement)
    {
        if (playerMovement is PlayerMovement movement)
        {
            if(_playerMovement != null)
            {
                RemoveMovement();
            }

            _inputHandler.SubscribeOnActionPerformed(InputType.Movement, movement.MovementPerformedListener);
            _inputHandler.SubscribeOnActionCanceled(InputType.Movement, movement.MovementStoppedListener);
            _playerMovement = movement;
        }
        else
        {
            Debug.LogWarning($"# Player movement can not be set. {playerMovement} has different type, expected : {nameof(PlayerMovement)}");
        }
    }

    public void SetBasicAttacks(PlayerBasicAttack attack)
    {
        _inputHandler.SubscribeOnActionPerformed(InputType.Attack, attack.AttackPerformedListener);
    }

    public void RemoveMovement()
    {
        _inputHandler.RemoveSubscriber(InputType.Movement, InputHandler.ActionState.Performed, _playerMovement.MovementPerformedListener);
        _inputHandler.RemoveSubscriber(InputType.Movement, InputHandler.ActionState.Canceled, _playerMovement.MovementStoppedListener);
        _playerMovement = null;
    }

    public void Disable()
    {
        _inputHandler.Disable();
    }
}
