using System;
using UnityEngine;

public class EnemyMovement : ICharacterMovement
{
    #region Fields 

    private float _currentSpeed;
    private float _maxSpeed = 2;

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private Transform _targetTransform;
    private Vector2 _targetDirection;

    #endregion


    #region Properties

    public bool IsMoving => !(SpeedMultiplier > 0f);
    public float SpeedMultiplier { get => _currentSpeed; set => _currentSpeed = value; }
    public Vector2 Direction => _targetDirection;

    public event EventHandler<SpeedChangedArgs> OnSpeedChanged;

    #endregion


    #region Methods

    #region Init

    public EnemyMovement(IEnemy source, IPlayer targetPlayer)
    {
        try
        {
            TrySetSource(source);
            TrySetTarget(targetPlayer);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        InitComponents();
        SpeedMultiplier = _maxSpeed;
    }

    private void InitComponents()
    {
        _rigidbody = _transform.GetComponent<Rigidbody2D>();
    }

    #endregion

    public void TrySetTarget(IPlayer targetPlayer)
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
        if (!IsMoving)
        {
            FollowPlayer();
        }
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    private void FollowPlayer()
    {
        _targetDirection = (_targetTransform.position - _transform.position).normalized;
        _rigidbody.velocity = SpeedMultiplier * _targetDirection;
    }

    private void TrySetSource(IEnemy source)
    {
        if (source is MonoBehaviour monoBehaviourEnemy)
        {
            SetSource(monoBehaviourEnemy.transform);
        }
        else
        {
            throw new NullReferenceException($"{nameof(source)} does not implement {nameof(MonoBehaviour)}. Source is null! - {ToString()}");
        }
    }

    private void SetSource(Transform source)
    {
        _transform = source;
    }



    #endregion
}
