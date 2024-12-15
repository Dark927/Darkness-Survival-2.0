using UnityEngine;

public class PlayerBody : CharacterBody
{

    #region Fields

    private PlayerAnimatorController _animatorController;

    #endregion


    #region Properties

    public PlayerAnimatorController AnimatorController { get => _animatorController; private set => _animatorController = value; }

    #endregion


    #region Methods 

    #region Init

    protected override void Init()
    {
        InitAnimation();
    }

    protected override void InitView()
    {
        View = new CharacterLookDirection(transform);
    }

    protected override void InitMovement()
    {
        Movement = new PlayerMovement(this);
        CharacterSpeed speed = new CharacterSpeed() { CurrentSpeedMultiplier = 4, MaxSpeedMultiplier = 4 };
        Movement.Speed.Set(speed);
    }

    private void InitAnimation()
    {
        Animator animator = GetComponentInChildren<Animator>();
        AnimatorController = new PlayerAnimatorController(animator, new PlayerAnimatorParameters());
    }

    protected override void InitReferences()
    {
        Movement.Speed.OnActualSpeedChanged += AnimatorController.SpeedUpdateListener;
    }

    protected override void ClearReferences()
    {
        Movement.Speed.OnActualSpeedChanged -= AnimatorController.SpeedUpdateListener;
    }

    #endregion


    private void FixedUpdate()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        Movement.Move();
        View.LookForward(Movement.Direction);
    }

    #endregion

}
