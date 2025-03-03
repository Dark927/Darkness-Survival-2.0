using System;
using Characters.Common.Movement;
using UnityEngine;

namespace Characters.Enemy
{
    public class EnemyMovement : EntityMovementBase
    {
        #region Fields 

        private Transform _transform;
        private Rigidbody2D _rigidbody;

        private Transform _targetTransform;
        private Vector2 _targetDirection;

        private EntitySpeed _speed;
        private EntityActionBlock _movementBlock;

        #endregion


        #region Properties

        public override EntitySpeed Speed => _speed;
        public override bool IsMoving => _rigidbody.velocity.sqrMagnitude > 0f;
        public override Vector2 Direction => _targetDirection;

        #endregion


        #region Methods

        #region Init

        public EnemyMovement(Transform bodyTransform, Transform targetTransform = null)
        {
            Init(bodyTransform, targetTransform);
            _speed = new EntitySpeed();
        }

        private void Init(Transform bodyTransform, Transform targetTransform)
        {
            try
            {
                _transform = bodyTransform;
                SetTarget(targetTransform);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            InitComponents();
        }

        private void InitComponents()
        {
            _rigidbody = _transform.GetComponent<Rigidbody2D>();
            _movementBlock = new EntityActionBlock();
            _speed = new EntitySpeed();
        }

        public override void ConfigureEventLinks()
        {
            _speed.OnVelocityUpdate += VelocityUpdateListener;
            _movementBlock.OnBlockFinish += _speed.SetMaxSpeedMultiplier;
        }

        public override void RemoveEventLinks()
        {
            _speed.OnVelocityUpdate -= VelocityUpdateListener;
            _movementBlock.OnBlockFinish -= _speed.SetMaxSpeedMultiplier;
        }

        #endregion

        public void SetTarget(Transform target)
        {
            _targetTransform = target;
        }

        public override void Move(Vector2 direction)
        {
            if (_movementBlock.IsBlocked)
            {
                return;
            }

            _speed.TryUpdateVelocity(direction);
        }

        public override void Stop()
        {
            _speed.Stop();
            _speed.ClearDirection();
        }

        public override void Block(int timeInMs)
        {
            Stop();
            _movementBlock.Block(timeInMs);
        }

        public override void Block()
        {
            Stop();
            _movementBlock.Block();
        }

        public override void Unblock()
        {
            base.Unblock();
            _movementBlock.Unblock();
        }

        public void FollowTarget()
        {
            _targetDirection = (_targetTransform.position - _transform.position).normalized;
            Move(_targetDirection);
        }


        private void VelocityUpdateListener(object sender, Vector2 velocity)
        {
            _rigidbody.velocity = _speed.Velocity;
        }

        #endregion
    }
}
