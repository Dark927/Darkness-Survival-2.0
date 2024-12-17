
using UnityEngine;

public class PlayerVisual : CharacterVisual
{
    public PlayerAnimatorController PlayerAnimController { get; set; }

    protected override void Init()
    { 
        base.Init();

        InitAnimation();
    }

    private void InitAnimation()
    {
        Animator animator = GetComponent<Animator>();
        PlayerAnimController = new PlayerAnimatorController(animator, new PlayerAnimatorParameters());
    }

    public override AnimatorController GetAnimatorController()
    {
        return PlayerAnimController;
    }
}


