using System.Threading;
using Characters.Common;
using Characters.Common.Movement;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Characters.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerCharacterMovement : EntityMovementBase
    {
        #region Fields 

        private Transform _playerTransform;
        private Rigidbody2D _rigidbody;
        private Vector2 _direction = Vector2.zero;

        private EntitySpeed _speed;
        private EntityActionBlock _movementBlock;

        private CancellationTokenSource _movementCts;

        #endregion


        #region Properties 

        public override bool IsMoving { get => _rigidbody.velocity.sqrMagnitude > 0f; }
        public override Vector2 Direction => _direction;
        public override EntitySpeed Speed => _speed;
        public override bool IsBlocked => (_movementBlock != null) && _movementBlock.IsBlocked;

        #endregion


        #region Methods 

        #region Init

        public PlayerCharacterMovement(EntityPhysicsBodyBase playerBody)
        {
            if (playerBody is MonoBehaviour playerMonoBehaviour)
            {
                // Init components which depends on Player firstly.

                _playerTransform = playerMonoBehaviour.transform;
                InitComponents(playerBody);
            }
            else
            {
                Debug.LogError("# Player does not implement a MonoBehaviour!");
            }
        }

        private void InitComponents(EntityPhysicsBodyBase playerBody)
        {
            _rigidbody = playerBody.Physics.Rigidbody2D;
            _movementBlock = new EntityActionBlock();
            _speed = new EntitySpeed();
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

        public override async UniTaskVoid MoveAsync(Vector2 direction)
        {
            StopMovementTask();
            _movementCts = new CancellationTokenSource();

            while (!_movementCts.Token.IsCancellationRequested)
            {
                Move(direction);
                await UniTask.WaitForFixedUpdate(_movementCts.Token);
            }
        }

        public override void Move(Vector2 direction)
        {
            _speed.SetMaxSpeedMultiplier();
            _direction = direction;

            if (_movementBlock.IsBlocked)
            {
                return;
            }

            _speed.TryUpdateVelocity(new Vector2(_direction.x, _direction.y).normalized);
            RaiseMovementPerformed();
        }

        private void StopMovementTask()
        {
            if (_movementCts == null)
            {
                return;
            }

            _movementCts.Cancel();
            _movementCts.Dispose();
            _movementCts = null;
        }

        public override void Stop()
        {
            StopMovementTask();
            _speed.Stop();
        }

        public override void Block(int timeInMs)
        {
            _speed.Stop();
            _movementBlock.Block(timeInMs);
        }

        public override void Block()
        {
            _speed.Stop();
            _movementBlock.Block();
        }

        public override void Unblock()
        {
            if (!_movementBlock.IsBlocked)
            {
                return;
            }
            _movementBlock.Unblock();
        }

        private void VelocityUpdateListener(object sender, Vector2 velocity)
        {
            _rigidbody.velocity = velocity;
        }


        #endregion
    }
}
