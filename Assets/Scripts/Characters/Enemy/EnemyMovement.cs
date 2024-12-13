using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyMovement : ICharacterMovement
{
    #region Fields 

    private float _maxSpeed = 2;
    private float _currentSpeedMultiplier;

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private Transform _targetTransform;
    private Vector2 _targetDirection;
    private Vector2 _velocity;

    private CharacterMovementBlock _blockLogic;

    #endregion


    #region Properties

    public bool IsMoving => _rigidbody.velocity.sqrMagnitude > 0f;
    public Vector2 Direction => _targetDirection;

    public float MaxSpeedMultiplier => _maxSpeed;

    public float ActualSpeed => Velocity.magnitude;

    public Vector2 Velocity
    {
        get => _velocity;
        private set
        {
            if (_velocity != value)
            {
                _velocity = value;
                _rigidbody.velocity = value;
                OnSpeedChanged?.Invoke(this, new SpeedChangedArgs(ActualSpeed, _maxSpeed));
            }
        }
    }

    public event EventHandler<SpeedChangedArgs> OnSpeedChanged;

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
        ResetSpeed();
    }

    private void InitComponents()
    {
        _rigidbody = _transform.GetComponent<Rigidbody2D>();
        _blockLogic = new CharacterMovementBlock(this); 
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
        _currentSpeedMultiplier = 0;
        Velocity = Vector2.zero;
    }

    public void BlockMovement(int timeInMs)
    {
        _blockLogic.OnBlockFinish += ResetSpeed;
        _blockLogic.BlockMovement(timeInMs);
    }

    public async UniTask UpdateSpeedMultiplierLinear(float targetSpeedMultiplier, int timeInMs, CancellationToken token)
    {
        float startSpeedMultiplier = _currentSpeedMultiplier;
        float endSpeedMultiplier = targetSpeedMultiplier;
        float elapsedTime = 0f;

        while (elapsedTime < timeInMs)
        {
            if(token.IsCancellationRequested)
            {
                _currentSpeedMultiplier = endSpeedMultiplier;
                return;
            }

            _currentSpeedMultiplier = Mathf.Lerp(startSpeedMultiplier, endSpeedMultiplier, elapsedTime / timeInMs);
            elapsedTime += Time.deltaTime * 1000f;

            await UniTask.Yield();
        }

        _currentSpeedMultiplier = endSpeedMultiplier;
    }

    private void FollowPlayer()
    {
        _targetDirection = (_targetTransform.position - _transform.position).normalized;
        Velocity = _currentSpeedMultiplier * _targetDirection;
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

    private void ResetSpeed()
    {
        _currentSpeedMultiplier = MaxSpeedMultiplier;
        _blockLogic.OnBlockFinish -= ResetSpeed;
    }

    #endregion
}
