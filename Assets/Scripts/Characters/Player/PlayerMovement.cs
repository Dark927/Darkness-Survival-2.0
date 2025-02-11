using Characters.Common.Movement;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Characters.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : CharacterMovementBase
    {
        #region Fields 

        private Transform _playerTransform;
        private Rigidbody2D _rigidbody;

        private Vector2 _direction;
        private Vector2 _savedDirection;
        private CharacterSpeed _speed;

        private CharacterActionBlock _movementBlock;

        #endregion


        #region Properties 

        public override bool IsMoving { get => _rigidbody.velocity.sqrMagnitude > 0f; }
        public override Vector2 Direction => _direction;
        public override CharacterSpeed Speed => _speed;
        public override bool IsBlocked => (_movementBlock != null) && _movementBlock.IsBlocked;

        #endregion


        #region Methods 

        #region Init

        public PlayerMovement(CharacterBodyBase playerBody)
        {
            if (playerBody is MonoBehaviour playerMonoBehaviour)
            {
                // Init components which depends on Player firstly.

                _playerTransform = playerMonoBehaviour.transform;
                InitComponents();
            }
            else
            {
                Debug.LogError("# Player does not implement a MonoBehaviour!");
            }
        }

        private void InitComponents()
        {
            _rigidbody = _playerTransform.gameObject.GetComponent<Rigidbody2D>();
            _movementBlock = new CharacterActionBlock();
            _speed = new CharacterSpeed();
        }

        public override void ConfigureEventLinks()
        {
            _speed.OnVelocityUpdate += VelocityUpdateListener;
        }

        public override void RemoveEventLinks()

        {
            _speed.OnVelocityUpdate -= VelocityUpdateListener;
        }

        #endregion

        public override void Move()
        {
            if (_movementBlock.IsBlocked)
            {
                return;
            }
            _speed.TryUpdateVelocity(new Vector2(_direction.x, _direction.y).normalized);
        }

        public override void Stop()
        {
            _speed.Stop();
        }

        public override void Block(int timeInMs)
        {
            SetStatic();
            _movementBlock.Block(timeInMs);
        }


        public override void Block()
        {
            SetStatic();
            _movementBlock.Block();
        }

        public override void Unblock()
        {
            if (!_movementBlock.IsBlocked)
            {
                return;
            }

            // ToDo : Test this isKinematic, coz it conflicts with Player Death IsKinematic!
            _rigidbody.isKinematic = false;
            _direction = _savedDirection;
            _movementBlock.Unblock();
        }

        public void MovementPerformedListener(InputAction.CallbackContext context)
        {
            _speed.SetMaxSpeedMultiplier();
            _savedDirection = context.ReadValue<Vector2>();

            if (!IsBlocked)
            {
                _direction = _savedDirection;
            }
        }

        public void MovementStoppedListener(InputAction.CallbackContext context)
        {
            Stop();
        }

        private void VelocityUpdateListener(object sender, Vector2 velocity)
        {
            _rigidbody.velocity = velocity;
        }

        private void SetStatic()
        {
            // do not use Stop method coz we want to save the speed velocity,
            // to continue moving if the input button is not released
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.isKinematic = true;
        }

        #endregion
    }
}
