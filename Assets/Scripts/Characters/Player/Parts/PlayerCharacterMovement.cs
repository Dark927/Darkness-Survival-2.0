using Characters.Common;
using Characters.Common.Movement;
using UnityEngine;
using Utilities.ErrorHandling;

namespace Characters.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerCharacterMovement : EntityMovementBase
    {
        private Rigidbody2D _rigidbody;
        private Vector2 _direction = Vector2.zero;

        private EntitySpeed _speed;
        private EntityActionBlock _movementBlock;

        public override bool IsMoving => _rigidbody.velocity.sqrMagnitude > 0f;
        public override Vector2 Direction => _direction;
        public override EntitySpeed Speed => _speed;
        public override bool IsBlocked => (_movementBlock != null) && _movementBlock.IsBlocked;

        public PlayerCharacterMovement(EntityPhysicsBodyBase playerBody)
        {
            if (playerBody is MonoBehaviour playerMonoBehaviour)
            {
                _rigidbody = playerBody.Physics.Rigidbody2D;
                _movementBlock = new EntityActionBlock();
                _speed = new EntitySpeed();
            }
            else
            {
                ErrorLogger.LogError("# Player does not implement a MonoBehaviour!");
            }
        }

        public override void ConfigureEventLinks()
        {
            _speed.OnVelocityUpdate += VelocityUpdateListener;
        }

        public override void RemoveEventLinks()
        {
            _speed.OnVelocityUpdate -= VelocityUpdateListener;
        }

        public override void Move(Vector2 direction)
        {
            _speed.SetMaxSpeedMultiplier();
            _direction = direction;

            if (_movementBlock.IsBlocked) return;

            _speed.TryUpdateVelocity(new Vector2(_direction.x, _direction.y).normalized);
            RaiseMovementPerformed();
        }

        public override void Stop()
        {
            base.Stop();
            DropSpeed();
        }

        public override void Block(int timeInMs)
        {
            DropSpeed();
            _movementBlock.Block(timeInMs);
        }

        public override void Block()
        {
            DropSpeed();
            _movementBlock.Block();
        }

        public override void Unblock()
        {
            if (!_movementBlock.IsBlocked) return;
            _movementBlock.Unblock();
        }

        private void VelocityUpdateListener(object sender, Vector2 velocity)
        {
            _rigidbody.velocity = velocity;
        }
    }
}
