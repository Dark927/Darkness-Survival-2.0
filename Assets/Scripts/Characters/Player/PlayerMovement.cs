using Characters.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Characters.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : ICharacterMovement, IDisposable
    {
        #region Fields 

        private Transform _playerTransform;
        private Rigidbody2D _rigidbody;

        private Vector2 _direction;
        private CharacterSpeed _speed;

        private CharacterMovementBlock _blockLogic;

        #endregion


        #region Properties 

        public bool IsMoving { get => _rigidbody.velocity.sqrMagnitude > 0f; }

        public Vector2 Direction => _direction;
        public ref CharacterSpeed Speed => ref _speed;


        #endregion


        #region Methods 

        #region Init

        public PlayerMovement(CharacterBody playerBody)
        {
            if (playerBody is MonoBehaviour playerMonoBehaviour)
            {
                // Init components which depends on Player firstly.

                _playerTransform = playerMonoBehaviour.transform;
                InitComponents();
                InitReferences();
            }
            else
            {
                Debug.LogError("# Player does not implement a MonoBehaviour!");
            }
        }

        private void InitComponents()
        {
            _rigidbody = _playerTransform.gameObject.GetComponent<Rigidbody2D>();
            _blockLogic = new CharacterMovementBlock(this);
            _speed = new CharacterSpeed();
        }

        private void InitReferences()
        {
            _speed.OnVelocityUpdate += VelocityUpdateListener;
        }

        public void Dispose()
        {
            _speed.OnVelocityUpdate -= VelocityUpdateListener;
        }

        #endregion

        public void Move()
        {
            if (_blockLogic.IsBlocked)
            {
                return;
            }
            _speed.TryUpdateVelocity(new Vector2(_direction.x, _direction.y).normalized);
        }

        public void Stop()
        {
            _speed.Stop();
        }

        public void BlockMovement(int timeInMs)
        {
            _blockLogic.BlockMovement(timeInMs);
        }

        public void MovementPerformedListener(InputAction.CallbackContext context)
        {
            _direction = context.ReadValue<Vector2>();
            _speed.SetMaxSpeedMultiplier();
        }

        public void MovementStoppedListener(InputAction.CallbackContext context)
        {
            Stop();
        }

        private void VelocityUpdateListener(object sender, Vector2 velocity)
        {
            _rigidbody.velocity = velocity;
        }

        private void ResetFields()
        {
            Stop();
        }

        #endregion
    }
}
