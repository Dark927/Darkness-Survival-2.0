using UnityEngine;

public class DefaultEnemyBody : CharacterBody
{
    #region Fields

    private EnemyAnimatorController _animatorController;

    #endregion


    #region Properties

    public EnemyAnimatorController AnimatorController { get => _animatorController; private set => _animatorController = value; }

    #endregion


    #region Methods

    #region Init

    protected override void InitView()
    {
        View = new CharacterLookDirection(transform);
    }

    protected override void InitMovement()
    {
        Movement = new EnemyMovement(this, FindObjectOfType<PlayerBody>());
    }

    protected override void InitAnimation()
    {
        Animator animator = GetComponent<Animator>();
        AnimatorController = new EnemyAnimatorController(animator, new EnemyAnimatorParameters());
    }

    protected override void InitReferences()
    {

    }

    #endregion

    private void FixedUpdate()
    {
        Movement.Move();
        View.LookForward(Movement.Direction);
    }

    #endregion

}
