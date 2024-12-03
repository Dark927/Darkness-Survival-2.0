using UnityEngine;

public class PlayerMovement : ICharacterMovement
{
    public float Speed = 4f;

    private Transform _playerTransform;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private float _horizontalMovement;
    private float _verticalMovement;

    public PlayerMovement(IPlayer player, Animator animator = null)
    {
        if (player is MonoBehaviour playerMonoBehaviour)
        {
            _playerTransform = playerMonoBehaviour.transform;
            _animator = animator;
            InitComponents();
        }
        else
        {
            Debug.LogError("# Player does not implement a MonoBehaviour!");
        }
    }

    public void Move()
    {
        // ---------------------------------------------------
        // ToDo : Implement Input Handler instead of these
        // ---------------------------------------------------
        
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");
        
        // ---------------------------------------------------
        
        _animator.SetFloat("Speed", new Vector2(_horizontalMovement, _verticalMovement).sqrMagnitude);

        if ((_horizontalMovement > 0 && _playerTransform.localScale.x < 0) || (_horizontalMovement < 0 && _playerTransform.localScale.x > 0))
        {
            Vector3 newScale = _playerTransform.localScale;
            newScale.x *= -1;

            _playerTransform.localScale = newScale;
        }

        Vector2 movementDirection = new Vector2(_horizontalMovement, _verticalMovement).normalized;
        Vector2 offset = new Vector2(_playerTransform.position.x + (movementDirection.x * Speed * Time.fixedDeltaTime),
                                      _playerTransform.position.y + (movementDirection.y * Speed * Time.fixedDeltaTime));

        _rigidbody.MovePosition(offset);
    }

    public void Stop()
    {
        throw new System.NotImplementedException();
    }

    private void InitComponents()
    {
        if (_animator == null)
        {
            _animator = _playerTransform.gameObject.GetComponentInChildren<Animator>();
        }
        _rigidbody = _playerTransform.gameObject.GetComponent<Rigidbody2D>();
    }
}
