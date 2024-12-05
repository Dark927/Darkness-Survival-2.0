using UnityEngine;


public class Nero : MonoBehaviour, IPlayer
{
    #region Fields

    private ICharacterMovement _movement;
    private PlayerBasicAttack _attack;
    private PlayerInput _playerInput;

    private Animator _animator;
    private CharacterAnimatorController _animatorController;

    #endregion


    #region Methods

    #region Init Methods

    private void Awake()
    {
        InitAnimation();
        InitInput();
        InitMovement();
        InitBasicAttacks();
    }

    private void InitAnimation()
    {
        _animator = GetComponentInChildren<Animator>();
        _animatorController = new CharacterAnimatorController(_animator, new PlayerAnimatorParameters());
    }

    private void InitMovement()
    {
        _movement = new PlayerMovement(this);
        _movement.OnSpeedChanged += _animatorController.SpeedUpdateListener;
        _playerInput.SetMovement(_movement);
    }

    private void InitBasicAttacks()
    {
        _attack = new PlayerBasicAttack();
        _playerInput.SetBasicAttacks(_attack);
        _attack.OnFastAttack += _animatorController.TriggerFastAttack;
        _attack.OnHeavyAttack += _animatorController.TriggerHeavyAttack;
    }

    private void InitInput()
    {
        IControlLayout controlLayout = new DefaultControlLayout();
        InputHandler inputHandler = new InputHandler(controlLayout);
        _playerInput = new PlayerInput(inputHandler);
    }

    #endregion

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        _movement.Move();
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
