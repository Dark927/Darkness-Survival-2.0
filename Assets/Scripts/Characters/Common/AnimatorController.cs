using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController<TParameters> where TParameters : IAnimatorParameters
{
    #region Fields 

    private TParameters _parameters;
    private Animator _animator;

    #endregion


    #region Properties

    public Animator Animator { get => _animator; private set => _animator = value; }
    public TParameters Parameters { get => _parameters; private set => _parameters = value; }

    #endregion


    #region Methods

    #region Init

    public AnimatorController(Animator characterAnimator, TParameters parameters)
    {
        Animator = characterAnimator;
        Parameters = parameters;
    }

    #endregion

    #endregion
}
