using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : ICharacterMovement
{
    #region Fields 

    private Transform _playerTransform;
    private Rigidbody2D _rigidbody;

    private float _maxSpeed = 3f;
    private Vector2 _direction;
    private Vector2 _velocity = Vector2.zero;

    public event EventHandler<SpeedChangedArgs> OnSpeedChanged;
    private CharacterMovementBlock _blockLogic;

    #endregion


    #region Properties 

    public bool IsMoving { get => _rigidbody.velocity.sqrMagnitude > 0f; }

    public float MaxSpeedMultiplier
    {
        get => _maxSpeed;
        set => _maxSpeed = value;
    }

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
                OnSpeedChanged?.Invoke(this, new SpeedChangedArgs(ActualSpeed, MaxSpeedMultiplier));
            }
        }
    }

    public Vector2 Direction => _direction;


    #endregion


    #region Methods 

    #region Init

    public PlayerMovement(CharacterBody playerBody)
    {
        if (playerBody is MonoBehaviour playerMonoBehaviour)
        {
            // Init components which depends on Player firstly.
            _playerTransform = playerMonoBehaviour.transform;

            ResetFields();
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
        _blockLogic = new CharacterMovementBlock(this);
    }

    #endregion

    private void ResetFields()
    {
        Velocity = Vector2.zero;
    }

    public void Move()
    {
        if (_blockLogic.IsBlocked)
        {
            return;
        }

        Vector2 calculatedVelocity = CalculateVelocity();
        Velocity = calculatedVelocity;
    }

    public void Stop()
    {
        _direction = Vector2.zero;
        Velocity = Vector2.zero;
    }

    public void MovementPerformedListener(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
    }

    public void MovementStoppedListener(InputAction.CallbackContext context)
    {
        Stop();
    }

    public void BlockMovement(int timeInMs)
    {
        _blockLogic.BlockMovement(timeInMs);
    }

    private Vector2 CalculateVelocity()
    {
        Vector2 movementDirection = new Vector2(_direction.x, _direction.y).normalized;

        return new Vector2(movementDirection.x * MaxSpeedMultiplier,
                           movementDirection.y * MaxSpeedMultiplier);
    }

    public UniTask UpdateSpeedMultiplierLinear(float targetSpeedMultiplier, int timeInMs, CancellationToken token)
    {
        // ToDo : implement
        throw new NotImplementedException();
    }

    #endregion
}
