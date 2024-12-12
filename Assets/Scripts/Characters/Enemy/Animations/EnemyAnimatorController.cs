using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : AnimatorController<EnemyAnimatorParameters>
{
    #region Properties

    public float Speed
    {
        get => Animator.GetFloat(Parameters.SpeedFieldName);
        set => Animator.SetFloat(Parameters.SpeedFieldName, value);
    }

    #endregion


    #region Methods

    #region Init
    public EnemyAnimatorController(Animator characterAnimator, EnemyAnimatorParameters parameters) : base(characterAnimator, parameters)
    {
    }

    #endregion

    #endregion
}
