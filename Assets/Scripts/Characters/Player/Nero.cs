using UnityEngine;
using UnityEngine.InputSystem;


public class Nero : MonoBehaviour, IPlayer
{
    private ICharacterMovement _movement;
    private Animator _animator;
    private CharacterAnimatorController _animatorController;


    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _animatorController = new CharacterAnimatorController(_animator, new PlayerAnimatorParameters());

        IControlLayout controlLayout = new DefaultControlLayout();
        InputHandler inputHandler = new InputHandler(controlLayout);

        _movement = new PlayerMovement(this, inputHandler);

        _movement.OnSpeedChanged += _animatorController.SpeedUpdateListener;
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
        _movement.Move();
    }
}
