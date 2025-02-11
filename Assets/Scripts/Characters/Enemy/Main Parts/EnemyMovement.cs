using Characters.Common.Movement;
using System;
using UnityEngine;

namespace Characters.Enemy
{
    public class EnemyMovement : CharacterMovementBase
    {
        #region Fields 

        private Transform _transform;
        private Rigidbody2D _rigidbody;

        private Transform _targetTransform;
        private Vector2 _targetDirection;

        private CharacterSpeed _speed;
        private CharacterActionBlock _movementBlock;

        #endregion


        #region Properties

        public override CharacterSpeed Speed => _speed;
        public override bool IsMoving => _rigidbody.velocity.sqrMagnitude > 0f;
        public override Vector2 Direction => _targetDirection;

        #endregion


        #region Methods

        #region Init

        public EnemyMovement(Transform bodyTransform, Transform targetTransform = null)
        {
            Init(bodyTransform, targetTransform);
            _speed = new CharacterSpeed();
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
            _movementBlock = new CharacterActionBlock();
            _speed = new CharacterSpeed();
        }

        public override void ConfigureEventLinks()
        {
            _speed.OnActualSpeedChanged += ActualSpeedChangedListener;
            _movementBlock.OnBlockFinish += _speed.SetMaxSpeedMultiplier;
        }

        public override void RemoveEventLinks()
        {
            _speed.OnActualSpeedChanged -= ActualSpeedChangedListener;
            _movementBlock.OnBlockFinish -= _speed.SetMaxSpeedMultiplier;
        }

        #endregion

        public void SetTarget(Transform target)
        {
            _targetTransform = target;
        }

        public override void Move()
        {
            if (!_movementBlock.IsBlocked && (_targetTransform != null))
            {
                FollowTarget();
            }
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

        private void FollowTarget()
        {
            _targetDirection = (_targetTransform.position - _transform.position).normalized;
            _speed.TryUpdateVelocity(_targetDirection);
        }


        private void ActualSpeedChangedListener(object sender, SpeedChangedArgs args)
        {
            _rigidbody.velocity = _speed.Velocity;
        }

        #endregion
    }
}
