using Characters.Common.Movement;
using Characters.Player.Weapons;
using System;
using UnityEngine;

namespace Characters.Player.Controls
{
    public class PlayerInput : IDisposable
    {
        private InputHandler _inputHandler;
        private PlayerCharacterMovement _playerMovement;
        private CharacterBasicAttack _playerBasicAttack;

        public PlayerInput(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        public void SetMovement(CharacterMovementBase playerMovement)
        {
            if (playerMovement is PlayerCharacterMovement movement)
            {
                if (_playerMovement != null)
                {
                    RemoveMovement();
                }

                _inputHandler.SubscribeOnActionPerformed(InputType.Movement, movement.MovementPerformedListener);
                _inputHandler.SubscribeOnActionCanceled(InputType.Movement, movement.MovementStoppedListener);
                _playerMovement = movement;
            }
            else
            {
                Debug.LogWarning($"# Player movement can not be set. {playerMovement} has different type, expected : {nameof(PlayerCharacterMovement)}");
            }
        }

        public void SetBasicAttacks(CharacterBasicAttack attack)
        {
            _playerBasicAttack = attack;
            _inputHandler.SubscribeOnActionPerformed(InputType.Attack, _playerBasicAttack.AttackPerformedListener);
        }

        public void RemoveMovement()
        {
            _inputHandler.RemoveSubscriber(InputType.Movement, InputHandler.ActionState.Performed, _playerMovement.MovementPerformedListener);
            _inputHandler.RemoveSubscriber(InputType.Movement, InputHandler.ActionState.Canceled, _playerMovement.MovementStoppedListener);
            _playerMovement = null;
        }

        public void RemoveBasicAttacks()
        {
            _inputHandler.RemoveSubscriber(InputType.Attack, InputHandler.ActionState.Performed, _playerBasicAttack.AttackPerformedListener);

        }

        public void RemoveReferences()
        {
            RemoveMovement();
            RemoveBasicAttacks();
        }

        public void Disable()
        {
            _inputHandler?.Disable();
        }

        public void Dispose()
        {
            RemoveMovement();
            Disable();
        }
    }
}
