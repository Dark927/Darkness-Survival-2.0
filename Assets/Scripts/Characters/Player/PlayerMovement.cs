using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : ICharacterMovement
{

    private Transform _playerTransform;
    private Animator _animator;
    private Rigidbody2D _rigidbody;

    private InputHandler _inputHandler;

    private float _actualSpeed;
    private float _maxSpeed = 6;
    private Vector2 _direction;

    public bool CanMove { get; private set; }
    public float CurrentSpeed { get => _actualSpeed; private set => _actualSpeed = value; }


    public PlayerMovement(IPlayer player, InputHandler inputHandler)
    {
        if (player is MonoBehaviour playerMonoBehaviour)
        {
            _actualSpeed = 0;
            _playerTransform = playerMonoBehaviour.transform;
            _inputHandler = inputHandler;

            // ToDo : Set Current Speed here and create separate methods for subscriptions

            _inputHandler.SubscribeOnActionPerformed(InputType.Movement, context => CanMove = true);
            _inputHandler.SubscribeOnActionCanceled(InputType.Movement, context => CanMove = false);

            InitComponents();
        }
        else
        {
            Debug.LogError("# Player does not implement a MonoBehaviour!");
        }
    }

    private void InitComponents()
    {
        if (_animator == null)
        {
            _animator = _playerTransform.gameObject.GetComponentInChildren<Animator>();
        }
        _rigidbody = _playerTransform.gameObject.GetComponent<Rigidbody2D>();
    }

    public void Move()
    {
        // ---------------------------------------------------
        // ToDo : Implement Input Handler instead of these
        // ---------------------------------------------------

        _direction = _inputHandler.RequestValueFromAction<Vector2>(InputType.Movement);

        if ((_direction.x > 0 && _playerTransform.localScale.x < 0) || (_direction.x < 0 && _playerTransform.localScale.x > 0))
        {
            Vector3 newScale = _playerTransform.localScale;
            newScale.x *= -1;

            _playerTransform.localScale = newScale;
        }

        Vector2 movementDirection = new Vector2(_direction.x, _direction.y).normalized;
        Vector2 offset = new Vector2(_playerTransform.position.x + (movementDirection.x * _maxSpeed * Time.fixedDeltaTime),
                                      _playerTransform.position.y + (movementDirection.y * _maxSpeed * Time.fixedDeltaTime));

        _rigidbody.MovePosition(offset);
    }

    public void Stop()
    {

    }


}
