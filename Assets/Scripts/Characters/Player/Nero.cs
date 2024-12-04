using UnityEngine;
using UnityEngine.InputSystem;

// (Tests)
public enum AttackType
{
    Reset = 0,
    Fast,
    Heavy
}

public class Nero : MonoBehaviour, IPlayer
{
    private ICharacterMovement _movement;
    private Animator _animator;


    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();

        IControlLayout controlLayout = new DefaultControlLayout();
        InputHandler inputHandler = new InputHandler(controlLayout);

        _movement = new PlayerMovement(this, inputHandler);
    }


    private void StopMovement(InputAction.CallbackContext context)
    {
        _movement.Stop();
        Debug.Log("stop");
    }

    private void Update()
    {
        Move();

    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }



    public void Move()
    {
        if (_movement.CanMove)
        {
            _movement.Move();
            _animator.SetFloat("Speed", _movement.CurrentSpeed);
        }
    }
}
