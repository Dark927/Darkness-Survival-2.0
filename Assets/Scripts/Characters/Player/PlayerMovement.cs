using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : ICharacterMovement
{
    #region Fields 

    private Transform _playerTransform;
    private Rigidbody2D _rigidbody;
    private InputHandler _inputHandler;

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

    public PlayerMovement(IPlayer player, InputHandler inputHandler)
    {
        if (player is MonoBehaviour playerMonoBehaviour)
        {
            // Init components which depends on Player firstly.
            _playerTransform = playerMonoBehaviour.transform;
            _inputHandler = inputHandler;

            ResetFields();
            InitComponents();
            ConfigureEvents();
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

    private void ResetFields()
    {
        _actualSpeed = 0;
        _isMovementBlocked = false;
    }

    private void ConfigureEvents()
    {
        _inputHandler.SubscribeOnActionPerformed(InputType.Movement, InputPerformedListener);
        _inputHandler.SubscribeOnActionCanceled(InputType.Movement, InputCanceledListener);
    }

    public void Move()
    {
        if (!IsAFK)
        {
            // -------------------------------------------------------------------
            // ToDo : Refactore this piece of code (create more separated methods)
            // -------------------------------------------------------------------

            _direction = _inputHandler.RequestValueFromAction<Vector2>(InputType.Movement);

            if ((_direction.x > 0 && _playerTransform.localScale.x < 0) || (_direction.x < 0 && _playerTransform.localScale.x > 0))
            {
                Vector3 newScale = _playerTransform.localScale;
                newScale.x *= -1;

                _playerTransform.localScale = newScale;
            }

            Vector2 movementDirection = new Vector2(_direction.x, _direction.y).normalized;
            Vector2 offset = new Vector2(_playerTransform.position.x + (movementDirection.x * CurrentSpeed * Time.fixedDeltaTime),
                                          _playerTransform.position.y + (movementDirection.y * CurrentSpeed * Time.fixedDeltaTime));

            _rigidbody.MovePosition(offset);
        }
    }

    public void Stop()
    {
        CurrentSpeed = 0;
    }

    private void InputPerformedListener(InputAction.CallbackContext context)
    {
        CurrentSpeed = _maxSpeed;
    }

    private void InputCanceledListener(InputAction.CallbackContext context)
    {
        Stop();
    }

    #endregion

}
