using UnityEngine;

public class EnemyVisual : CharacterVisual
{
    public EnemyAnimatorController AnimController { get; set; }

    protected override void Init()
    {
        base.Init();
        InitAnimation();
    }

    private void InitAnimation()
    {
        Animator animator = GetComponent<Animator>();
        AnimController = new EnemyAnimatorController(animator, new EnemyAnimatorParameters());
    }

    public override AnimatorController GetAnimatorController()
    {
        return AnimController;
    }
}
