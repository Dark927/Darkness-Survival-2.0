using Characters.Interfaces;
using System;
using UnityEngine;
namespace Characters.Enemy
{
    public class EnemyMovement : ICharacterMovement, IDisposable
    {
        #region Fields 

        private Transform _transform;
        private Rigidbody2D _rigidbody;

        private Transform _targetTransform;
        private Vector2 _targetDirection;

        private CharacterSpeed _speed;
        private CharacterMovementBlock _blockLogic;

        #endregion


        #region Properties

        public ref CharacterSpeed Speed => ref _speed;
        public bool IsMoving => _rigidbody.velocity.sqrMagnitude > 0f;
        public Vector2 Direction => _targetDirection;

        #endregion


        #region Methods

        #region Init

        public EnemyMovement(Transform bodyTransform, Transform targetTransform = null)
        {
            Init(bodyTransform, targetTransform);
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
            InitReferences();
        }

        private void InitComponents()
        {
            _rigidbody = _transform.GetComponent<Rigidbody2D>();
            _blockLogic = new CharacterMovementBlock(this);
            _speed = new CharacterSpeed();
        }

        private void InitReferences()
        {
            _speed.OnActualSpeedChanged += ActualSpeedChangedListener;
            _blockLogic.OnBlockFinish += _speed.SetMaxSpeedMultiplier;
        }

        #endregion

        public void SetTarget(Transform target)
        {
            _targetTransform = target;
        }

        public void Move()
        {
            if (!_blockLogic.IsBlocked && (_targetTransform != null))
            {
                FollowTarget();
            }
        }

        public void Stop()
        {
            _speed.Stop();
            _speed.ClearDirection();
        }

        public void BlockMovement(int timeInMs)
        {
            _blockLogic.BlockMovement(timeInMs);
        }


        public void Dispose()
        {
            _speed.OnActualSpeedChanged -= ActualSpeedChangedListener;
            _blockLogic.OnBlockFinish -= _speed.SetMaxSpeedMultiplier;
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