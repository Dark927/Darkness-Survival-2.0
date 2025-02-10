
using System;
using UnityEngine;

public class PlayerAnimatorController : AnimatorController
{

    #region Properties

    public new PlayerAnimatorParameters Parameters { get => base.Parameters as PlayerAnimatorParameters; }

    public float Speed
    {
        get => Animator.GetFloat(Parameters.SpeedFieldName);
        set => Animator.SetFloat(Parameters.SpeedFieldName, value);
    }

    public PlayerAttackType AttackType
    {
        get => (PlayerAttackType)Animator.GetInteger(Parameters.AttackTypeFieldName);
        set => Animator.SetInteger(Parameters.AttackTypeFieldName, (int)value);
    }

    #endregion


    #region Methods

    #region Init

    public PlayerAnimatorController(Animator characterAnimator, PlayerAnimatorParameters parameters) : base(characterAnimator, parameters)
    {
    }

    #endregion


    public void TriggerFastAttack(object sender, EventArgs args)
    {
        AttackType = PlayerAttackType.Fast;
        Animator.SetTrigger(Parameters.AttackTriggerName);
    }

    public void TriggerHeavyAttack(object sender, EventArgs args)
    {
        AttackType = PlayerAttackType.Heavy;
        Animator.SetTrigger(Parameters.AttackTriggerName);
    }

    public void TriggerDeath()
    {
        Animator.SetTrigger(Parameters.AttackTriggerName);
    }

    public void SpeedUpdateListener(object sender, SpeedChangedArgs args)
    {
        Speed = args.CurrentSpeed;
    }

    #endregion
}
