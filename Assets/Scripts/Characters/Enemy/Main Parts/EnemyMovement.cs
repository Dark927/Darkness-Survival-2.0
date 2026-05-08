using System;
using Characters.Common.Movement;
using Characters.Enemy.Settings;
using UnityEngine;

namespace Characters.Enemy
{
    public class EnemyMovement : EntityMovementBase
    {
        #region Fields 

        private Transform _transform;
        private Rigidbody2D _rigidbody;

        private Transform _targetTransform;

        private Vector2 _moveDirection;
        private Vector2 _lookDirection;

        private EntitySpeed _speed;
        private EntityActionBlock _movementBlock;

        private SwarmMovementSettingsData _swarmSettings;
        private static readonly Collider2D[] _neighborsBuffer = new Collider2D[15];

        #endregion


        #region Properties

        public override EntitySpeed Speed => _speed;
        public override bool IsMoving => _rigidbody.velocity.sqrMagnitude > 0f;
        public override Vector2 Direction => _lookDirection;

        #endregion


        #region Methods

        #region Init

        public EnemyMovement(Transform bodyTransform, SwarmMovementSettingsData swarmSettings, Transform targetTransform = null)
        {
            _swarmSettings = swarmSettings;
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

        public override void ResetState()
        {
            base.ResetState();

            _moveDirection = Vector2.zero;
            _lookDirection = Vector2.zero;
            _targetTransform = null;

            if (_rigidbody != null)
            {
                _rigidbody.velocity = Vector2.zero;
            }
        }

        public void FollowTarget()
        {
            if (_targetTransform == null) return;

            _lookDirection = (_targetTransform.position - _transform.position).normalized;

            Vector2 separationVector = CalculateSeparationVector();
            _moveDirection = (_lookDirection + separationVector).normalized;

            Move(_moveDirection);
        }

        /// <summary>
        /// Calculates an inverse push-back vector from nearby entities to simulate organic swarm separation.
        /// </summary>
        private Vector2 CalculateSeparationVector()
        {
            if (_swarmSettings == null) return Vector2.zero;

            Vector2 separation = Vector2.zero;

            int count = Physics2D.OverlapCircleNonAlloc(
                _transform.position,
                _swarmSettings.SeparationRadius,
                _neighborsBuffer,
                _swarmSettings.EnemyLayerMask);

            for (int i = 0; i < count; i++)
            {
                Transform neighbor = _neighborsBuffer[i].transform;

                if (neighbor == _transform) continue;

                Vector2 difference = (Vector2)_transform.position - (Vector2)neighbor.position;
                float distance = difference.magnitude;

                if (distance > 0 && distance < _swarmSettings.SeparationRadius)
                {
                    float pushStrength = 1f - (distance / _swarmSettings.SeparationRadius);
                    separation += (difference.normalized * pushStrength);
                }
            }

            return separation * _swarmSettings.SeparationWeight;
        }

        private void VelocityUpdateListener(object sender, Vector2 velocity)
        {
            _rigidbody.velocity = _speed.Velocity;
        }

        #endregion
    }
}
