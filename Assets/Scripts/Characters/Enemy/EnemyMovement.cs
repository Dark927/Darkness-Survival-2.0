using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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

        public EnemyMovement(CharacterBody body, CharacterBody targetPlayer)
        {
            try
            {
                TrySetBody(body);
                TrySetTarget(targetPlayer);
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

        public void TrySetTarget(CharacterBody targetPlayer)
        {
            if (targetPlayer is MonoBehaviour monoBehaviourPlayer)
            {
                SetTarget(monoBehaviourPlayer.transform);
            }
            else
            {
                throw new NullReferenceException($"{nameof(targetPlayer)} does not implement {nameof(MonoBehaviour)}. Target is null! - {ToString()}");
            }
        }

        public void SetTarget(Transform target)
        {
            _targetTransform = target;
        }

        public void Move()
        {
            if (!_blockLogic.IsBlocked)
            {
                FollowPlayer();
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

        private void FollowPlayer()
        {
            _targetDirection = (_targetTransform.position - _transform.position).normalized;
            _speed.TryUpdateVelocity(_targetDirection);
        }

        private void TrySetBody(CharacterBody body)
        {
            if (body is MonoBehaviour monoBehaviourEnemy)
            {
                SetBody(monoBehaviourEnemy.transform);
            }
            else
            {
                throw new NullReferenceException($"{nameof(body)} does not implement {nameof(MonoBehaviour)}. Source is null! - {ToString()}");
            }
        }

        private void SetBody(Transform body)
        {
            _transform = body;
        }

        private void ActualSpeedChangedListener(object sender, SpeedChangedArgs args)
        {
            _rigidbody.velocity = _speed.Velocity;
        }

        #endregion
    }
}