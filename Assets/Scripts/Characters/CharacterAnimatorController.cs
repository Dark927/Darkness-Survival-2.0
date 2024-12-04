
using System;
using UnityEngine;

public class CharacterAnimatorController
{
    private IAnimatorParameters _parameters;
    private Animator _animator;

    public CharacterAnimatorController(Animator characterAnimator, IAnimatorParameters parameters)
    {
        _animator = characterAnimator;
        _parameters = parameters;
    }

    public float Speed
    {
        get => _animator.GetFloat(_parameters.SpeedFieldName);
        set => _animator.SetFloat(_parameters.SpeedFieldName, value);
    }

    public PlayerAttackType AttackType
    {
        get => (PlayerAttackType)_animator.GetInteger(_parameters.AttackTypeFieldName);
        set => _animator.SetInteger(_parameters.AttackTypeFieldName, (int)value);
    }

    public void TriggerAttack()
    {
        _animator.SetTrigger(_parameters.AttackTriggerName);
    }

    public void TriggerDeath()
    {
        _animator.SetTrigger(_parameters.AttackTriggerName);
    }

    public void SpeedUpdateListener(object sender, SpeedChangedArgs args)
    {
        Speed = args.CurrentSpeed;
    }
}
