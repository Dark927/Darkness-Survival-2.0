using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : ICharacterMovement
{
    #region Fields 

    private Transform _playerTransform;
    private Rigidbody2D _rigidbody;

    private float _actualSpeed;
    private float _maxSpeed = 3f;
    private Vector2 _direction;

    private bool _isMovementBlocked;

    public event EventHandler<SpeedChangedArgs> OnSpeedChanged;

    #endregion


    #region Properties 

    public bool IsAFK { get => !(CurrentSpeed > 0f); }

    public float CurrentSpeed
    {
        get => _actualSpeed;
        private set
        {
            if (_actualSpeed != value)
            {
                _actualSpeed = value;
                OnSpeedChanged?.Invoke(this, new SpeedChangedArgs(_actualSpeed, _maxSpeed));
            }
        }
    }

    public Vector2 Direction => _direction;


    #endregion


    #region Methods 

    #region Init

    public PlayerMovement(IPlayer player)
    {
        if (player is MonoBehaviour playerMonoBehaviour)
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
    }

    #endregion

    private void ResetFields()
    {
        _actualSpeed = 0;
        _isMovementBlocked = false;
    }

    public void Move()
    {
        if (IsAFK || _isMovementBlocked)
        {
            return;
        }

        Vector2 position = CalculatePosition();
        _rigidbody.MovePosition(position);

        // ToDo : Test movement and remove this line of code in the future
        //_playerTransform.Translate(CurrentSpeed * Time.fixedDeltaTime * _direction.normalized);
    }

    public void Stop()
    {
        CurrentSpeed = 0;
    }

    public void MovementPerformedListener(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
        CurrentSpeed = _maxSpeed;
    }

    public void MovementStoppedListener(InputAction.CallbackContext context)
    {
        Stop();
    }

    private Vector2 CalculatePosition()
    {
        Vector2 movementDirection = new Vector2(_direction.x, _direction.y).normalized;

        return new Vector2(_playerTransform.position.x + (movementDirection.x * CurrentSpeed * Time.fixedDeltaTime),
                           _playerTransform.position.y + (movementDirection.y * CurrentSpeed * Time.fixedDeltaTime));
    }

    #endregion
}
