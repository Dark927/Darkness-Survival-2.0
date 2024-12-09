using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : ICharacterMovement
{
    #region Fields 

    private Transform _playerTransform;
    private Rigidbody2D _rigidbody;

    private float _actualSpeed;
    private float _maxSpeed = 4f;
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


        if (!IsLookingForward())
        {
            SwitchLookDirection();
        }

        Vector2 offset = CalculateOffset();
        _rigidbody.MovePosition(offset);
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

    private bool IsLookingForward()
    {
        float scaleX = _playerTransform.localScale.x;

        bool correctLeftLookSide = (_direction.x < 0) && (scaleX < 0);
        bool correctRightLookSide = (_direction.x > 0) && (scaleX > 0);
        bool previousLookSide = (_direction.x == 0);

        return previousLookSide || (correctLeftLookSide || correctRightLookSide);
    }

    private void SwitchLookDirection()
    {
        Vector3 newScale = _playerTransform.localScale;
        newScale.x *= -1;

        _playerTransform.localScale = newScale;
    }

    private Vector2 CalculateOffset()
    {
        Vector2 movementDirection = new Vector2(_direction.x, _direction.y).normalized;

        return new Vector2(_playerTransform.position.x + (movementDirection.x * CurrentSpeed * Time.fixedDeltaTime),
                           _playerTransform.position.y + (movementDirection.y * CurrentSpeed * Time.fixedDeltaTime));
    }

    #endregion
}
